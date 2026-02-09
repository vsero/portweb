using System.Text.Json;
using System.Text.Json.Serialization;

namespace Vsero.UTrack;

internal static class JsonOptions
{

    public static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower) }
    };

}
