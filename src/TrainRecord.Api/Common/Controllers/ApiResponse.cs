namespace TrainRecord.Api.Common.Controllers;

public record ApiOkResponse(object Data, string TraceId)
{
    public DateTime DateTime = DateTime.Now;
    public int StatusCode = 200;
}

public record ApiCreatedResponse(object Data, string TraceId)
{
    public DateTime DateTime = DateTime.Now;
    public int StatusCode = 201;
}
