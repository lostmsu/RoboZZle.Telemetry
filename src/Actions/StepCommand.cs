namespace RoboZZle.Telemetry.Actions;

public sealed class StepCommand: ISolverCommand {
    public const char PREFIX = '+';

    public static StepCommand Instance { get; } = new();

    StepCommand() { }

    public override string ToString() => "+";

#if !JAVA
    public void Replay(IProgramHistory programHistory) {
        // has no effect on program editing history
    }
#endif
}