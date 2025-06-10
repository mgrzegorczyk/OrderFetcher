namespace OrderFetcher.Domain.Entities;

public class Order
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
    public ICollection<OrderItem> Items { get; set; }
    
    public int BillingAddressId { get; set; }
    public Address BillingAddress { get; set; }

    public int? ShippingAddressId { get; set; }
    public Address? ShippingAddress { get; set; }
}