using System.ComponentModel.DataAnnotations;

namespace EFCoreShowcase.DTOs;

public record AddressDto
{
    public long Id { get; init; }
    public string Street { get; init; } = null!;
    public string City { get; init; } = null!;
    public string State { get; init; } = null!;
    public string Country { get; init; } = null!;
    public string PostalCode { get; init; } = null!;
}

public record CreateAddressDto
{
    [Required, StringLength(200)]
    public string Street { get; init; } = null!;

    [Required, StringLength(100)]
    public string City { get; init; } = null!;

    [Required, StringLength(100)]
    public string State { get; init; } = null!;

    [Required, StringLength(100)]
    public string Country { get; init; } = null!;

    [Required, StringLength(20)]
    public string PostalCode { get; init; } = null!;

    [Required]
    public long CustomerId { get; init; }
}
