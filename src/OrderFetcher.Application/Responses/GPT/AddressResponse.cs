namespace OrderFetcher.Application.Responses.GPT;

public class AddressResponse
{
    public string FullName { get; set; }
    public string Street { get; set; }
    public string BuildingNumber { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}