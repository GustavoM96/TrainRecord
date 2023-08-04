using TrainRecord.Core.Interfaces;

namespace TrainRecord.Core.Commum.Bases;

public class EntityId<TEntity> where TEntity : IEntity
{
    public Guid Value { get; }

    public EntityId(Guid guid)
    {
        Value = guid;
    }

    public static implicit operator Guid(EntityId<TEntity> idBase) => idBase.Value;
}
