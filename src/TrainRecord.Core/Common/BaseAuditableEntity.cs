namespace TrainRecord.Core.Commum;

public class BaseAuditableEntity
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? LastModifiedAt { get; set; }

    public string? LastModifiedBy { get; set; }
}
