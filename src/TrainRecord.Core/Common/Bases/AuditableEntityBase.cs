using TrainRecord.Core.Interfaces;

namespace TrainRecord.Core.Commum.Bases;

public abstract class AuditableEntityBase : IAuditableEntityBase
{
    public DateTime CreatedAt { get; private set; }

    public string? CreatedBy { get; private set; }

    public DateTime? LastModifiedAt { get; private set; }

    public string? LastModifiedBy { get; private set; }

    public void SetCreatedInfo(string createdBy, DateTime createdAt)
    {
        CreatedBy = createdBy;
        CreatedAt = createdAt;
    }

    public void SetUpdatedInfo(string lastModifiedBy, DateTime lastModifiedAt)
    {
        LastModifiedBy = lastModifiedBy;
        LastModifiedAt = lastModifiedAt;
    }
}
