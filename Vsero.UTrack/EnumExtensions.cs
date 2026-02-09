using System.Text.Json;

namespace Vsero.UTrack;

internal static class EnumExtensions
{
    public static string ToSnakeCase(this Enum value)
    {
        return JsonNamingPolicy.SnakeCaseLower.ConvertName(value.ToString());
    }

}
