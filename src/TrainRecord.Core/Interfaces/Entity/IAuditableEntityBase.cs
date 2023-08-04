namespace TrainRecord.Core.Interfaces;

public interface IAuditableEntityBase
{
    void AddDomainEevnt(IDomainEvent domainEvent);
    void ClearDomainEvent();
    public IReadOnlyList<IDomainEvent> DomainEvents { get; }
    void SetUpdatedInfo(string lastModifiedBy, DateTime lastModifiedAt);
    void SetCreatedInfo(string createdBy, string createdByEmail, DateTime createdAt);
    DateTime CreatedAt { get; }

    string? CreatedBy { get; }

    DateTime? LastModifiedAt { get; }

    string? LastModifiedBy { get; }
}
