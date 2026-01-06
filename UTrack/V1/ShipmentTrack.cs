namespace UTrack.V1;

public class ShipmentTrack
{
    public Guid Uuid { get; set; } = Guid.NewGuid();

    public DateTime DeclaredTimestamp { get; set; } = DateTime.MinValue;

    public string Label { get; set; } = string.Empty;
    
    public string Title { get; set; } = string.Empty;

    public string BlNumber { get; set; } = string.Empty;

    public decimal TotalQuantity { get; set; }

    public HandlingSummary HandlingSummary { get; set; } = new HandlingSummary();

    public IEnumerable<HandlingEvent> HandlingEvents { get; set; } = [];
}
