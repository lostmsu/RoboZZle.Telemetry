namespace RoboZZle.Telemetry;

using System.Runtime.Serialization;

[DataContract]
public sealed class TelemetryBag {
    [DataMember]
    public required List<SolutionTelemetry> Solutions { get; init; } = new();
}