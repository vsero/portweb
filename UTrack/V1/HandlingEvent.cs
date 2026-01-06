using System.Text.Json.Serialization;

namespace UTrack.V1;

public enum HandlingEventType
{
    [JsonStringEnumMemberName("unloading")]
    Unloading,
    [JsonStringEnumMemberName("loading")]
    Loading,
    [JsonStringEnumMemberName("gate_in")]
    GateIn,
    [JsonStringEnumMemberName("gate_out")]
    GateOut,
    [JsonStringEnumMemberName("yard_in")]
    YardIn,
    [JsonStringEnumMemberName("yard_out")]
    YardOut
}

public class HandlingEvent
{
    public DateTime Timestamp { get; set; } = DateTime.MinValue;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public HandlingEventType Type { get; set; }

    public HandlingEventLocation Location { get; set; } = new HandlingEventLocation();

    public decimal Quantity { get; set; } = 0m;


    [JsonIgnore]
    public string TypeRu => Type switch
    {
        HandlingEventType.Unloading => "Выгрузка",
        HandlingEventType.Loading => "Погрузка",
        HandlingEventType.GateIn => "Ввоз на территорию",
        HandlingEventType.GateOut => "Вывоз с территории",
        HandlingEventType.YardIn => "Помещен на хранение",
        HandlingEventType.YardOut => "Снят с хранения",
        _ => "<Неизвестное событие>"
    };

}
