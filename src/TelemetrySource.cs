namespace RoboZZle.Telemetry;

using System.Runtime.Serialization;

[DataContract]
public sealed class TelemetrySource {
    [DataMember]
    public required string Product { get; set; }
    [DataMember]
    public required string Version { get; set; }
    [DataMember]
    public bool IsTest { get; set; }

    public override bool Equals(object? obj) {
        if (obj is not TelemetrySource other)
            return false;

        return this.Product == other.Product && this.Version == other.Version;
    }

    public override int GetHashCode() {
        return this.Product.GetHashCode() * 12122419 ^ this.Version.GetHashCode();
    }
}