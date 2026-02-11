namespace Vsero.UTrcak.V1;

public class CargoShipment
{
    public Guid Uuid { get; set; } = Guid.NewGuid();
    public DateTime DeclaredTimestamp { get; set; } = DateTime.MinValue;
    public string Label { get; set; } = string.Empty;    
    public string Title { get; set; } = string.Empty;
    public string BlNumber { get; set; } = string.Empty;
    public decimal TotalQuantity { get; set; }
    public bool IsStarted { get; set; }
    public DateTime IsStartedTimestamp { get; set; }
    public bool IsFinished { get; set; }
    public DateTime IsFinishedTimestamp { get; set; }
    public CargoFlow CargoFlow { get; set; } = CargoFlow.FromLand;
    public string VesselCallTitle { get; set; } = string.Empty;
    public CargoHandlingEvent[] HandlingEvents { get; set; } = [];
}

//DLTU9018145