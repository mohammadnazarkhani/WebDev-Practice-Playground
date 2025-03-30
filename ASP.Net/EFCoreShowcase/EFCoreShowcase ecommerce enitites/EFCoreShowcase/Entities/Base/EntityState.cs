namespace EFCoreShowcase.Entities.Base;

public interface IEntityState
{
    EntityStatus Status { get; }
    void SetStatus(EntityStatus status);
}

public enum EntityStatus
{
    Unchanged,
    Added,
    Modified,
    Deleted
}
