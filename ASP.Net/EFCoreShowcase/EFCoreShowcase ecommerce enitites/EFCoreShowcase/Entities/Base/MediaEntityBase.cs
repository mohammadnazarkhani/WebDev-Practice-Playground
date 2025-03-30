using EFCoreShowcase.Entities.Base;
using EFCoreShowcase.Entities.Interfaces;

public abstract class MediaEntityBase : IEntityState, IAuditableEntity
{
    private readonly List<DomainEvent> _domainEvents = new();

    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }

    public EntityStatus Status { get; private set; } = EntityStatus.Unchanged;

    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void SetStatus(EntityStatus status)
    {
        Status = status;
    }

    public void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public virtual EntityValidationResult Validate()
    {
        var errors = new List<string>();

        if (CreatedAt > DateTime.UtcNow)
            errors.Add("Created date cannot be in the future");

        if (UpdatedAt.HasValue && UpdatedAt.Value < CreatedAt)
            errors.Add("Updated date cannot be earlier than created date");

        return errors.Any()
            ? EntityValidationResult.Failure(errors)
            : EntityValidationResult.Success();
    }

    protected virtual void OnSaving() { }
    protected virtual void OnSaved() { }
}
