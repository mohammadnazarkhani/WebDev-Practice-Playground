namespace EFCoreShowcase.Core.Interfaces;

public interface ICategoryValidator
{
    Task<bool> CategoryExistsAsync(int categoryId, CancellationToken cancellation = default);
    Task<bool> IsNameUniqueAsync(string name, CancellationToken cancellation = default);
    bool CategoryExists(int categoryId);
    bool IsNameUnique(string name);
}
