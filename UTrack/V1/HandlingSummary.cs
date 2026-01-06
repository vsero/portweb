namespace UTrack.V1;

public class HandlingSummary
{
    public bool IsStarted { get; set; }
    public DateTime IsStartedTimestamp { get; set; }
    public bool IsFinished { get; set; }
    public DateTime IsFinishedTimestamp { get; set; }
    public string CargoFlow { get; set; } = string.Empty;
    public string VesselCallTitle { get; set; } = string.Empty;
}
