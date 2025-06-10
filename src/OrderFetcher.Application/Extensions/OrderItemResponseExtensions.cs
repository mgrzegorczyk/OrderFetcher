using OrderFetcher.Application.Responses.GPT;
using OrderFetcher.Domain.Entities;

namespace OrderFetcher.Application.Extensions;

public static class OrderItemResponseExtensions
{
    public static OrderItem ToOrderItem(this OrderItemResponse itemResponse)
    {
        if (itemResponse == null)
            return null;

        return new OrderItem
        {
            ProductName = itemResponse.ProductName,
            Quantity = itemResponse.Quantity,
            Price = itemResponse.Price
        };
    }
}