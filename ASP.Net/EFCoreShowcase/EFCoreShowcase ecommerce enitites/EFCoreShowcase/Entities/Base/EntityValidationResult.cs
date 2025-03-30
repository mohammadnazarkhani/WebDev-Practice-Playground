namespace EFCoreShowcase.Entities.Base;

public class EntityValidationResult
{
    public bool IsValid { get; }
    public IReadOnlyList<string> Errors { get; }

    private EntityValidationResult(bool isValid, IReadOnlyList<string> errors)
    {
        IsValid = isValid;
        Errors = errors;
    }

    public static EntityValidationResult Success() => new(true, Array.Empty<string>());
    public static EntityValidationResult Failure(IReadOnlyList<string> errors) => new(false, errors);
}
