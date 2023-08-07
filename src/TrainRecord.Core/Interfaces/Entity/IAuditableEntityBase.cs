using MediatR;

namespace TrainRecord.Core.Interfaces;

public interface IAuditableEntityBase
{
    DateTime CreatedAt { get; }
    string? CreatedBy { get; }
    DateTime? LastModifiedAt { get; }
    string? LastModifiedBy { get; }

    void SetUpdatedInfo(string lastModifiedBy, DateTime lastModifiedAt);
    void SetCreatedInfo(string createdBy, DateTime createdAt);
}
