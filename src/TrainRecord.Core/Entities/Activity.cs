using TrainRecord.Core.Commum;

namespace TrainRecord.Core.Entities;

public class Activity : BaseAuditableEntity
{
    public string Name { get; set; }
}
