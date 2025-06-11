using OrderFetcher.Application.Dtos;
using OrderFetcher.Domain.Entities;

namespace OrderFetcher.Application.Extensions;

public static class OrderItemExtensions
{
    public static OrderItemDto ToDto(this OrderItem orderItem)
    {
        if (orderItem == null) return null;

        return new OrderItemDto
        {
            ProductName = orderItem.ProductName,
            Quantity = orderItem.Quantity,
            Price = orderItem.Price
        };
    }
}