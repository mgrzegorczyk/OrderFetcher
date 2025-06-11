using OrderFetcher.Application.Dtos;
using OrderFetcher.Domain.Entities;

namespace OrderFetcher.Application.Extensions;

public static class OrderExtensions
{
    public static OrderDto ToDto(this Order order)
    {
        if (order == null) return null;

        return new OrderDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            OrderDate = order.OrderDate,
            CreatedAt = order.CreatedAt,
            Amount = order.Amount,
            TotalAmount = order.TotalAmount,
            Currency = order.Currency,
            ShippingMethod = order.ShippingMethod,
            PaymentMethod = order.PaymentMethod,
            BillingAddress = order.BillingAddress?.ToDto(),
            ShippingAddress = order.ShippingAddress?.ToDto(),
            Items = order.Items?.Select(item => item.ToDto()).ToList()
        };
    }
}