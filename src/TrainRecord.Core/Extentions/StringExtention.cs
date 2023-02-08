using System;
using Mapster;
using TrainRecord.Core.Common;

namespace TrainRecord.Core.Extentions
{
    public static class StringExtention
    {
        public static bool EqualsIgnoreCase(this string text, string valueToCompare)
        {
            return string.Equals(text, valueToCompare, StringComparison.OrdinalIgnoreCase);
        }

        public static bool HasValue(this string text)
        {
            return !string.IsNullOrWhiteSpace(text);
        }
    }
}
