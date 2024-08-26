namespace TrainRecord.Core.Extensions;

public static class BooleanExtension
{
    public static bool ValueOrFalse(this bool? boolean)
    {
        return boolean is not null && boolean.Value;
    }
}
