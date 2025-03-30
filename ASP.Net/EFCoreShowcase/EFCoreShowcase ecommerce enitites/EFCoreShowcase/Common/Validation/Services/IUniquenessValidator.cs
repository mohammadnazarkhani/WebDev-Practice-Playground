namespace EFCoreShowcase.Common.Validation.Services;

public interface IUniquenessValidator
{
    Task<bool> IsUniqueAsync<TEntity>(string propertyName, string value, long? excludeId = null) where TEntity : class;
    bool IsUnique<TEntity>(string propertyName, string value, long? excludeId = null) where TEntity : class;
}
