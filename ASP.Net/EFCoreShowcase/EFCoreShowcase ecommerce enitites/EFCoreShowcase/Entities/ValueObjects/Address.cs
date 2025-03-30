namespace EFCoreShowcase.Entities.ValueObjects;

public record PostalAddress
{
    public string Street { get; }
    public string City { get; }
    public string State { get; }
    public string Country { get; }
    public string PostalCode { get; }

    public PostalAddress(string street, string city, string state, string country, string postalCode)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Street cannot be empty");

        Street = street;
        City = city;
        State = state;
        Country = country;
        PostalCode = postalCode;
    }
}
