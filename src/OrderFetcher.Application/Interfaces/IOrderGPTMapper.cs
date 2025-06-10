using OrderFetcher.Domain.Entities;

namespace OrderFetcher.Application.Interfaces;

public interface IOrderGPTMapper
{
    public Task<Order?> MapEmailBodyToOrderAsync(string emailBody, CancellationToken cancellationToken);
}