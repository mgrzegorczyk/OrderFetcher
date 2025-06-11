namespace OrderFetcher.Application.Dtos;

public class OrderDto
{
    public int Id { get; set; }
    public string OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal Amount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; }
    public string ShippingMethod { get; set; }
    public string PaymentMethod { get; set; }
    public AddressDto BillingAddress { get; set; }
    public AddressDto? ShippingAddress { get; set; }
    public IEnumerable<OrderItemDto> Items { get; set; }
}