using OrderFetcher.Application.Responses.GPT;
using OrderFetcher.Domain.Entities;

namespace OrderFetcher.Application.Extensions;

public static class OrderResponseExtensions
{
    public static Order ToOrder(this OrderResponse orderResponse)
    {
        if (orderResponse == null)
            return null;

        return new Order
        {
            OrderNumber = orderResponse.OrderNumber,
            OrderDate = DateTime.TryParse(orderResponse.OrderDate, out var parsedDate) ? parsedDate : default,
            Amount = orderResponse.Amount,
            TotalAmount = orderResponse.TotalAmount,
            Currency = orderResponse.Currency,
            ShippingMethod = orderResponse.ShippingMethod,
            PaymentMethod = orderResponse.PaymentMethod,
            Items = orderResponse.Items?.Select(i => i.ToOrderItem()).ToList(),
            BillingAddress = orderResponse.BillingAddress?.ToAddress(),
            ShippingAddress = orderResponse.ShippingAddress?.ToAddress(),
            CreatedAt = DateTime.UtcNow
        };
    }
}