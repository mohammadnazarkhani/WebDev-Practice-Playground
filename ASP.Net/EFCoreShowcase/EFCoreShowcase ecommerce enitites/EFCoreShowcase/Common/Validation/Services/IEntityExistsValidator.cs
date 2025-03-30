namespace EFCoreShowcase.Common.Validation.Services;

public interface IEntityExistsValidator
{
    Task<bool> ExistsAsync<TEntity>(long id) where TEntity : class;
    bool Exists<TEntity>(long id) where TEntity : class;
}
