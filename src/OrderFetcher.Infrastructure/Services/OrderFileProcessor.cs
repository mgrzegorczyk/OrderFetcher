using OrderFetcher.Application.Interfaces;
using OrderFetcher.Domain.Entities;
using OrderFetcher.Domain.Models;

namespace OrderFetcher.Infrastructure.Services;

public class OrderFileProcessor : IOrderFileProcessor
{
    private readonly IEmailParser _emailParser;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderGPTMapper _orderGptMapper;

    public OrderFileProcessor(IEmailParser emailParser, IOrderRepository orderRepository, IOrderGPTMapper orderGPTMapper)
    {
        _emailParser = emailParser;
        _orderRepository = orderRepository;
        _orderGptMapper = orderGPTMapper;
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

        if (!Directory.Exists(errorDirectory))
        {
            Directory.CreateDirectory(errorDirectory);
        }

        if (!Directory.Exists(processedDirectory))
        {
            Directory.CreateDirectory(processedDirectory);
        }

        var emlFiles = Directory.GetFiles(inputDirectory, "*.eml");

        var tasks = emlFiles.Select(filePath =>
                ProcessFileAsync(filePath, errorDirectory, processedDirectory, cancellationToken))
            .ToList();

        cancellationToken.ThrowIfCancellationRequested();

        await Task.WhenAll(tasks);
    }

    private async Task ProcessFileAsync(
        string filePath,
        string errorDirectory,
        string processedDirectory,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        try
        {
            var parsedEmail = await _emailParser.ParseEmailFromFileAsync(filePath);
            var order = await _orderGptMapper.MapEmailBodyToOrderAsync(parsedEmail.Body, cancellationToken);

            if (order == null)
            {
                throw new InvalidOperationException("Failed to map email body to Order.");
            }

            await _orderRepository.AddAsync(order);

            Console.WriteLine($"Processed file: {filePath}");

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