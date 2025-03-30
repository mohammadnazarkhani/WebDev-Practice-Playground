using EFCoreShowcase.Entities.Base;
using EFCoreShowcase.Entities.Interfaces;

public abstract class AuditableEntity : IAuditableEntity, IEntityState
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }

    public EntityStatus Status { get; private set; } = EntityStatus.Unchanged;

    public void SetStatus(EntityStatus status)
    {
        Status = status;
    }

    public virtual void OnSaving() { }
    public virtual void OnSaved() { }
}
