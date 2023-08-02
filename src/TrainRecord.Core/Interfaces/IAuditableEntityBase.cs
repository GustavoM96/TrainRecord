using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;

namespace TrainRecord.Core.Interfaces;

public interface IAuditableEntityBase
{
    Guid Id { get; init; }

    DateTime CreatedAt { get; set; }

    string? CreatedBy { get; set; }

    DateTime? LastModifiedAt { get; set; }

    string? LastModifiedBy { get; set; }
}
