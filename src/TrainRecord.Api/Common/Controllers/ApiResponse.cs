namespace TrainRecord.Api.Common.Controllers;

public record ApiOkResponse(object Data, string TraceId)
{
    public DateTime DateTime { get; set; } = DateTime.Now;
    public int StatusCode { get; set; } = 200;
}

public record ApiCreatedResponse(object Data, string TraceId)
{
    public DateTime DateTime { get; set; } = DateTime.Now;
    public int StatusCode { get; set; } = 201;
}
