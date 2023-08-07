namespace TrainRecord.Application.Responses;

public class GetUserActivityResponse
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
}
