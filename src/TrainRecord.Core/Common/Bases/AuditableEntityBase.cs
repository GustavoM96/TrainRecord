using TrainRecord.Core.Interfaces.Repositories;

namespace TrainRecord.Core.Commum.Bases;

public class AuditableEntityBase<TEntity> : IAuditableEntityBase
    where TEntity : IAuditableEntityBase
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public EntityId<TEntity> EntityId => new(Id);

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? LastModifiedAt { get; set; }

    public string? LastModifiedBy { get; set; }
}

public class EntityId<TEntity> where TEntity : IAuditableEntityBase
{
    public Guid Value { get; }

    public EntityId(Guid guid)
    {
        Value = guid;
    }

    public static implicit operator Guid(EntityId<TEntity> idBase) => idBase.Value;
}
