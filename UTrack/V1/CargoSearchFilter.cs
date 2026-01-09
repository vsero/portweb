namespace UTrack.V1;

public enum CargoFlow
{
    Inbound,
    Outbound
}

public class CargoSearchFilter
{
    public string SearchString { get; set; } = string.Empty;
    public CargoFlow CargoFlow { get; set; } = CargoFlow.Inbound;
    public bool IsContainerUnit { get; set; } = true;
    public bool IsEmptyContainerUnit { get; set; } = false;
}
