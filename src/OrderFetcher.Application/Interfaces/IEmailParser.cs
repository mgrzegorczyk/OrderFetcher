using OrderFetcher.Domain.Models;

namespace OrderFetcher.Application.Interfaces;

public interface IEmailParser
{
    Task<ParsedEmail> ParseEmailFromFileAsync(string filePath, CancellationToken cancellationToken = default);
}