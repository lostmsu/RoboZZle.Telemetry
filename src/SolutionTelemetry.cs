namespace RoboZZle.Telemetry;

using System.Runtime.Serialization;

[DataContract]
public sealed class SolutionTelemetry {
    [DataMember]
    public int PuzzleID { get; set; }
    [DataMember]
    public required string StartingProgram { get; init; }
    [DataMember]
    public List<SessionLog> Sessions { get; private set; } = [];

    [DataMember]
    public required TelemetrySource Source { get; init; }
}