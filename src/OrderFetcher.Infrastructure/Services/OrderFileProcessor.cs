using Microsoft.Extensions.DependencyInjection;
using OrderFetcher.Application.Interfaces;
using OrderFetcher.Domain.Entities;
using OrderFetcher.Domain.Models;

namespace OrderFetcher.Infrastructure.Services;

public class OrderFileProcessor : IOrderFileProcessor
{
    private readonly IEmailParser _emailParser;
    private readonly IOrderGPTMapper _orderGptMapper;
    private readonly IServiceScopeFactory _scopeFactory;

    public OrderFileProcessor(IEmailParser emailParser, IOrderRepository orderRepository,
        IOrderGPTMapper orderGPTMapper, IServiceScopeFactory scopeFactory)
    {
        _emailParser = emailParser;
        _orderGptMapper = orderGPTMapper;
        _scopeFactory = scopeFactory;
    }

    public async Task ProcessOrderFilesAsync(
        string inputDirectory,
        string errorDirectory,
        string processedDirectory,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!Directory.Exists(inputDirectory))
        {
            throw new DirectoryNotFoundException($"Input directory does not exist: {inputDirectory}");
        }

        Directory.CreateDirectory(errorDirectory);
        Directory.CreateDirectory(processedDirectory);

        var emlFiles = Directory.GetFiles(inputDirectory, "*.eml");
        var semaphore = new SemaphoreSlim(5); // Limit do 5 równoczesnych operacji
        var tasks = new List<Task>();

        foreach (var filePath in emlFiles)
        {
            await semaphore.WaitAsync(cancellationToken);

            tasks.Add(Task.Run(async () =>
            {
                try
                {
                    await ProcessFileAsync(filePath, errorDirectory, processedDirectory, cancellationToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to process file: {filePath}, error: {ex.Message}");
                }
                finally
                {
                    semaphore.Release();
                }
            }, cancellationToken));
        }

        await Task.WhenAll(tasks);
    }

    private async Task ProcessFileAsync(
        string filePath,
        string errorDirectory,
        string processedDirectory,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Console.WriteLine($"[Start] Processing file: {filePath} - {DateTime.Now:HH:mm:ss.fff}");
        try
        {
            var parsedEmail = await _emailParser.ParseEmailFromFileAsync(filePath);
            var order = await _orderGptMapper.MapEmailBodyToOrderAsync(parsedEmail.Body, cancellationToken);

            if (order == null)
            {
                throw new InvalidOperationException("Failed to map email body to Order.");
            }

            using var scope = _scopeFactory.CreateScope();
            var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

            await orderRepository.AddAsync(order);

            Console.WriteLine($"[Success] Processed file: {filePath} - {DateTime.Now:HH:mm:ss.fff}");

            MoveFileToDirectory(filePath, processedDirectory);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while processing file {filePath}: {ex.Message}");
            MoveFileToDirectory(filePath, errorDirectory);
        }
    }

    private void MoveFileToDirectory(string filePath, string directoryName)
    {
        try
        {
            var fileName = Path.GetFileName(filePath);
            var destinationPath = Path.Combine(directoryName, fileName);
            File.Move(filePath, destinationPath, overwrite: true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to move file {filePath} to directory {directoryName}: {ex.Message}");
        }
    }
}