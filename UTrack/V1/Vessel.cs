namespace UTrack.V1
{
    public class Vessel
    {
        public Guid Uuid { get; set; } = Guid.NewGuid();
        
        public string Title { get; set; } = string.Empty;

        public string Callsign { get; set; } = string.Empty;
    }
}