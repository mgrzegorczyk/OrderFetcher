namespace OrderFetcher.Application.Responses.GPT;

public class OrderResponse
{
    public string OrderNumber { get; set; }
    public string OrderDate { get; set; }
    public decimal Amount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; }
    public string ShippingMethod { get; set; }
    public string PaymentMethod { get; set; }
    public List<OrderItemResponse> Items { get; set; }
    public AddressResponse BillingAddress { get; set; }
    public AddressResponse? ShippingAddress { get; set; }
}