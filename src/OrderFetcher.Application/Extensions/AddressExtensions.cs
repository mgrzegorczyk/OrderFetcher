using OrderFetcher.Application.Dtos;
using OrderFetcher.Domain.Entities;

namespace OrderFetcher.Application.Extensions;

public static class AddressExtensions
{
    public static AddressDto ToDto(this Address address)
    {
        if (address == null) return null;

        return new AddressDto
        {
            FullName = address.FullName,
            Street = address.Street,
            BuildingNumber = address.BuildingNumber,
            PostalCode = address.PostalCode,
            City = address.City
        };
    }
}