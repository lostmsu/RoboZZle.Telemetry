namespace RoboZZle.Telemetry.Actions;

public sealed class StopCommand: ISolverCommand {
    public const char PREFIX = '=';

    public static StopCommand Instance { get; } = new();

    StopCommand() { }

    public override string ToString() => "=";

#if !JAVA
    public void Replay(IProgramHistory programHistory) {
        // has no effect on program editing history
    }
#endif
}