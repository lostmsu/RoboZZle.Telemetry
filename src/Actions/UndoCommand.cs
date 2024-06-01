namespace RoboZZle.Telemetry.Actions;

public sealed class UndoCommand: ISolverCommand {
    public const char PREFIX = 'U';

    public static UndoCommand Instance { get; } = new();

    UndoCommand() { }

    public override string ToString() => "U";

#if !JAVA
    public void Replay(IProgramHistory programHistory) {
        if (programHistory == null)
            throw new ArgumentNullException(nameof(programHistory));
        programHistory.Undo();
    }
#endif
}