using System.Text.RegularExpressions;

namespace UTrack;

public static partial class DateExtentions
{
    public static string ToStringOrEmpty(this DateTime dt, string format = "dd.MM.yyyy HH:mm")
    {
        if (dt != DateTime.MinValue)
            return dt.ToString(format);

        return FormatRegex().Replace(format, "-");
    }

    [GeneratedRegex("[a-zA-Z]")]
    private static partial Regex FormatRegex();
}
