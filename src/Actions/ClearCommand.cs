namespace RoboZZle.Telemetry.Actions;

public sealed class ClearCommand: ISolverCommand {
    public const char PREFIX = '_';

    public static ClearCommand Instance { get; } = new();

    ClearCommand() { }

    public override string ToString() => PREFIX + "";

#if !JAVA
    public void Replay(IProgramHistory programHistory) {
        var emptyProgram = programHistory.CurrentProgram.Empty();
        programHistory.Add(emptyProgram);
    }
#endif
}