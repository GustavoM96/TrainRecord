namespace TrainRecord.Core.Commum;

public class BaseAuditableEntity
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public DateTime CreatedAt { get; init; }

    public string? CreatedBy { get; init; }

    public DateTime? LastModifiedAt { get; init; }

    public string? LastModifiedBy { get; init; }
}
