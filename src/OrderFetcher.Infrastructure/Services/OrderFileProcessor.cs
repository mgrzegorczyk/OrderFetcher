using OrderFetcher.Application.Interfaces;
using OrderFetcher.Domain.Entities;
using OrderFetcher.Domain.Models;

namespace OrderFetcher.Infrastructure.Services;

public class OrderFileProcessor : IOrderFileProcessor
{
    private readonly IEmailParser _emailParser;
    private readonly IOrderRepository _orderRepository;

    public OrderFileProcessor(IEmailParser emailParser, IOrderRepository orderRepository)
    {
        _emailParser = emailParser;
        _orderRepository = orderRepository;
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
            var order = MapEmailToOrder(parsedEmail);

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

    private Order MapEmailToOrder(ParsedEmail parsedEmail)
    {
        // TODO map parsed email to order by chatgpt
        return new Order
        {
            OrderNumber = new Random().Next(1000, 9999),
            OrderDate = DateTime.Now,
            Amount = 100,
            TotalAmount = 120,
            Currency = "USD",
            ShippingMethod = "Standard",
            PaymentMethod = "CreditCard",
            Items = new List<OrderItem>
            {
                new OrderItem
                {
                    ProductName = "Sample Product From File",
                    Quantity = 1,
                    Price = 100
                }
            },
            BillingAddress = new Address
            {
                Street = "Sample Street",
                City = "Sample City",
                PostalCode = "12345",
            }
        };
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