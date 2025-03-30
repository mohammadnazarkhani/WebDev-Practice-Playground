namespace EFCoreShowcase.Common.Exceptions;

public class DomainException : Exception
{
    public string[] Errors { get; }

    public DomainException(string message) : base(message)
    {
        Errors = new[] { message };
    }

    public DomainException(string[] errors) : base(string.Join(", ", errors))
    {
        Errors = errors;
    }
}
