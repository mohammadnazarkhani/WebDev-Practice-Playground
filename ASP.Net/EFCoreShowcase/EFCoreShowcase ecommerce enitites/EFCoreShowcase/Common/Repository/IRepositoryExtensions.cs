using EFCoreShowcase.Common.Pagination;
using EFCoreShowcase.Common.Specifications;

namespace EFCoreShowcase.Common.Repository;

public static class IRepositoryExtensions
{
    public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(
        this IRepository<T> repository,
        ISpecification<T> spec,
        int pageNumber,
        int pageSize) where T : class
    {
        var totalItems = await repository.CountAsync(spec);
        var items = await repository.ListWithSpecAsync(spec);
        return new PaginatedResult<T>(items, totalItems, pageNumber, pageSize);
    }
}
