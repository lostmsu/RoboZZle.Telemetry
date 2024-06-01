namespace RoboZZle.Telemetry.Actions;

public sealed class PauseCommand: ISolverCommand {
    public const char PREFIX = '|';

    public static PauseCommand Instance { get; } = new();

    PauseCommand() { }

    public override string ToString() => "||";
#if !JAVA
    public void Replay(IProgramHistory programHistory) {
        // has no effect on program editing history
    }
#endif
}