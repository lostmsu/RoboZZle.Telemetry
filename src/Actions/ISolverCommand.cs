namespace RoboZZle.Telemetry.Actions;

public interface ISolverCommand {
#if !JAVA
    /// <summary>
    /// Converts command to its string representation.
    /// </summary>
    string ToString();

    /// <summary>
    /// Replays command on top of a program editing history
    /// </summary>
    void Replay(IProgramHistory programHistory);
#endif
}