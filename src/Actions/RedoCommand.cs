namespace RoboZZle.Telemetry.Actions;

public sealed class RedoCommand: ISolverCommand {
    public const char PREFIX = 'R';

    public static RedoCommand Instance { get; } = new();

    RedoCommand() { }

    public override string ToString() => "R";

#if !JAVA
    public void Replay(IProgramHistory programHistory) {
        if (programHistory == null)
            throw new ArgumentNullException(nameof(programHistory));

        programHistory.Redo();
    }
#endif
}