using OrderFetcher.Application.Responses.GPT;
using OrderFetcher.Domain.Entities;

namespace OrderFetcher.Application.Extensions;

public static class AddressResponseExtensions
{
    public static Address ToAddress(this AddressResponse addressResponse)
    {
        if (addressResponse == null)
            return null;

        return new Address
        {
            FullName = addressResponse.FullName,
            Street = addressResponse.Street,
            BuildingNumber = addressResponse.BuildingNumber,
            PostalCode = addressResponse.PostalCode,
            City = addressResponse.City,
            PhoneNumber = addressResponse.PhoneNumber,
            Email = addressResponse.Email
        };
    }
}