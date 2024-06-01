namespace RoboZZle.Telemetry;

using System.Globalization;
using System.Runtime.Serialization;

using RoboZZle.Telemetry.Actions;

/// <summary>
/// Represents user action during puzzle solution session
/// </summary>
[DataContract]
public sealed class SessionLogEntry {
    /// <summary>
    /// Command, executed by user
    /// </summary>
    [DataMember]
    public required string Command { get; init; }
    /// <summary>
    /// Time, when command was executed
    /// </summary>
    [DataMember]
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Makes a deep copy of this object
    /// </summary>
    public SessionLogEntry Copy() => new() { Command = this.Command, TimeStamp = this.TimeStamp };

    /// <summary>
    /// Converts this object to its string representation.
    /// </summary>
    public override string ToString() {
        return string.Format(CultureInfo.InvariantCulture,
                             "{0}=>{1}",
                             this.TimeStamp, this.Command);
    }
#if !JAVA
    public void Replay(InMemoryProgramHistory programHistory) {
        var command = SolverCommand.Parse(this.Command);
        command.Replay(programHistory);
    }
#endif
}