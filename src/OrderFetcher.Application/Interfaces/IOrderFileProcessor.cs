namespace OrderFetcher.Application.Interfaces;

public interface IOrderFileProcessor
{
    public Task ProcessOrderFilesAsync(string inputDirectory, string errorDirectory, string processedDirectory, CancellationToken cancellationToken);
}