using System;
using System.ComponentModel.DataAnnotations.Schema;
using MediatR;
using TrainRecord.Core.Interfaces;

namespace TrainRecord.Core.Commum.Bases;

public class AuditableEntityBase<TEntity> : IAuditableEntityBase where TEntity : class, IEntity
{
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEevnt(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void SetCreatedInfo(string createdBy, string createdByEmail, DateTime createdAt)
    {
        CreatedBy = createdBy;
        CreatedAt = createdAt;
    }

    public void SetUpdatedInfo(string lastModifiedBy, DateTime lastModifiedAt)
    {
        LastModifiedBy = lastModifiedBy;
        LastModifiedAt = lastModifiedAt;
    }

    public void ClearDomainEvent()
    {
        _domainEvents.Clear();
    }

    public Guid Id { get; init; } = Guid.NewGuid();

    public EntityId<TEntity> EntityId => new(Id);

    public DateTime CreatedAt { get; private set; }

    public string? CreatedBy { get; private set; }

    public DateTime? LastModifiedAt { get; private set; }

    public string? LastModifiedBy { get; private set; }
}

public class EntityId<TEntity> where TEntity : class, IEntity
{
    public Guid Value { get; }

    public EntityId(Guid guid)
    {
        Value = guid;
    }

    public static implicit operator Guid(EntityId<TEntity> idBase) => idBase.Value;
}
