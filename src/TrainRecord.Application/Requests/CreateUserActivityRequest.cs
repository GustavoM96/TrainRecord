namespace TrainRecord.Application.Requests;

public record CreateUserActivityRequest(
    Guid? TeacherId,
    int Weight,
    int Repetition,
    int Serie,
    string? TrainGroup
) { }
