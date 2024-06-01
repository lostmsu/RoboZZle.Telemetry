namespace RoboZZle.Telemetry;

using System.Runtime.Serialization;

/// <summary>
/// Represents puzzle solution editing session log.
/// </summary>
[DataContract]
public sealed class SessionLog {
    /// <summary>
    /// ID of the puzzle being edited
    /// </summary>
    [DataMember]
    public int PuzzleID { get; set; }
    /// <summary>
    /// Solution actions
    /// </summary>
    [DataMember]
    public List<SessionLogEntry> Entries { get; private set; } = new();
    /// <summary>
    /// Session start time
    /// </summary>
    [DataMember]
    public DateTime StartTime { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Makes a deep copy of this object
    /// </summary>
    public SessionLog Copy() => new() {
        PuzzleID = this.PuzzleID,
        Entries = this.Entries.Select(e => e.Copy()).ToList(),
        StartTime = this.StartTime,
    };

#if !JAVA
    /// <summary>
    /// Replays solution editing steps on top of the specified program. Returns resulting program.
    /// </summary>
    public Program Replay(Program startingProgram) {
        var programHistory = new InMemoryProgramHistory(startingProgram);
        int commandCounter = 0;
        foreach (var entry in this.Entries) {
            entry.Replay(programHistory);
            commandCounter++;
        }

        return programHistory.CurrentProgram;
    }
#endif
}