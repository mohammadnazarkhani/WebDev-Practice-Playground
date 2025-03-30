namespace EFCoreShowcase.DTOs.Common;

public interface IDto
{
    long Id { get; }
}

public interface IAuditableDto : IDto
{
    DateTime CreatedAt { get; }
    DateTime? UpdatedAt { get; }
}
