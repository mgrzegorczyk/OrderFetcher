namespace OrderFetcher.Domain.Entities;

public class Address
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Street { get; set; }
    public string BuildingNumber { get; set; } 
    public string PostalCode { get; set; }
    public string City { get; set; } 
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
}