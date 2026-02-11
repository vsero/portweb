using System.Text.Json.Serialization;

namespace Vsero.UTrcak.V1;

public class VesselCall
{
    private VesselCallStage? _stage;

    public Guid Uuid { get; set; }
    public string VoyageNumber { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateTime DeclaredTimestamp { get; set; }
    public DateOnly EstimatedArrivalDate { get; set; }
    public DateTime StartWaitingTimestamp { get; set; }
    public DateTime StartUnloadingTimestamp { get; set; }
    public DateTime StartLoadingTimestamp { get; set; }
    public DateTime DepartureTimestamp { get; set; }
    public bool IsFinished { get; set; }
    public DateTime IsFinishedTimestampp { get; set; }
    public Vessel Vessel { get; set; } = new Vessel();
    public Yard? UnloadingBerth { get; set; }
    public Yard? LoadingBerth { get; set; }
    public Port PortOfDeparture { get; set; } = new Port();
    public Port PortOfDestination { get; set; } = new Port();
    public HandlingTotal[] HandlingTotals { get; set; } = [];


    [JsonIgnore]
    public Yard? Berth => this switch
    {
        { StartLoadingTimestamp.Year: > 1 } => LoadingBerth,
        { StartUnloadingTimestamp.Year: > 1 } => UnloadingBerth,
        _ => null
    };

    [JsonIgnore]
    public VesselCallStage Stage => _stage ??= GetStage();

    [JsonIgnore]
    public UnloadingStats UnStats => new(this);

   private VesselCallStage GetStage()
    {
        return this switch
        {
            { DepartureTimestamp.Year: > 1 } => VesselCallStage.Departured,
            { StartLoadingTimestamp.Year: > 1 } => VesselCallStage.Handling,
            { StartUnloadingTimestamp.Year: > 1 } => VesselCallStage.Handling,
            { StartWaitingTimestamp.Year: > 1 } => VesselCallStage.Waiting,
            _ => VesselCallStage.Expected
        };
    }

    public class HandlingTotal
    {
        public HandlingType? HandlingType { get; set; }
        public CargoType? CargoType { get; set; }
        public bool IsEmptyContainerUnit { get; set; }
        public long DeclaredPieces { get; set; }
        public double DeclaredWeight { get; set; }
        public long HandledPieces { get; set; }
        public double HandledWeight { get; set; }
    }

    public class UnloadingStats
    {
        public long IsoDeclared;
        public long IsoHandled;
        public int IsoPercent;
        public double GenDeclared;
        public double GenHandled;
        public int GenPercent;
        public int TotalPercent;
        public bool IsUnloaded;
        public VesselHandlingStatus Status = VesselHandlingStatus.None;

        public UnloadingStats(VesselCall call)
        {
            var unloadingTotals = call.HandlingTotals.Where(t => t.HandlingType == HandlingType.Unloading);
            
            var isoUnloading = unloadingTotals.Where(t => t.CargoType == CargoType.IsoContainer);
            IsoDeclared = isoUnloading?.Sum(t => t.DeclaredPieces) ?? 0;
            IsoHandled = isoUnloading?.Sum(t => t.HandledPieces) ?? 0;
            IsoPercent = IsoDeclared > 0 ? (int)Math.Round((double)IsoHandled / IsoDeclared * 100) : 0;

            var genUnloading = unloadingTotals.Where(t => t.CargoType == CargoType.General);
            GenDeclared = genUnloading?.Sum(t => t.DeclaredWeight) ?? 0;
            GenHandled = genUnloading?.Sum(t => t.HandledWeight) ?? 0;
            GenPercent = GenDeclared > 0 ? (int)Math.Round(GenHandled / GenDeclared * 100) : 0;

            double totalPercent = (IsoPercent + GenPercent) / 2;
            TotalPercent = (int)Math.Round(totalPercent);

            IsUnloaded = IsoDeclared == IsoHandled && GenDeclared <= GenHandled;

            Status = this switch
            {
                { IsoPercent: 100 } when GenPercent == 100 => VesselHandlingStatus.Done,
                { IsoHandled: > 0 } or { GenHandled: > 0 } => VesselHandlingStatus.InProgress,
                { IsoDeclared: > 0 } or { GenDeclared: > 0 } => VesselHandlingStatus.Expected,
                _ => VesselHandlingStatus.None
            };

        }
    }

}

public enum HandlingType
{
    Unloading,
    Loading
}

public enum CargoType
{
    IsoContainer,
    General
}

public enum VesselCallStage
{
    Expected,
    Waiting,
    Handling,
    Departured
}

public enum VesselHandlingStatus
{
    None,
    Expected,
    InProgress,
    Done
}