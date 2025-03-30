using System.ComponentModel.DataAnnotations;

namespace EFCoreShowcase.DTOs;

public record CustomerDto
{
    public long Id { get; init; }
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string PhoneNumber { get; init; } = null!;
    public ICollection<AddressDto> Addresses { get; init; } = new List<AddressDto>();
}

public record CreateCustomerDto
{
    [Required, StringLength(50)]
    public string FirstName { get; init; } = null!;

    [Required, StringLength(50)]
    public string LastName { get; init; } = null!;

    [Required, EmailAddress]
    public string Email { get; init; } = null!;

    [Required, Phone]
    public string PhoneNumber { get; init; } = null!;
}

public record UpdateCustomerDto
{
    [StringLength(50)]
    public string? FirstName { get; init; }

    [StringLength(50)]
    public string? LastName { get; init; }

    [Phone]
    public string? PhoneNumber { get; init; }
}
