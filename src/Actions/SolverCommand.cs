namespace RoboZZle.Telemetry.Actions;

#if !JAVA
public static class SolverCommand {
    /// <summary>
    /// Parses a string representation of some <see cref="ISolverCommand"/>
    /// </summary>
    public static ISolverCommand Parse(string command) {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        if (command.Length == 0)
            throw new FormatException();

        return command[0] switch {
            EditCommand.PREFIX => EditCommand.Parse(command),
            PlayCommand.PREFIX => PlayCommand.Parse(command),
            PauseCommand.PREFIX => PauseCommand.Instance,
            RedoCommand.PREFIX => RedoCommand.Instance,
            UndoCommand.PREFIX => UndoCommand.Instance,
            StepCommand.PREFIX => StepCommand.Instance,
            StopCommand.PREFIX => StopCommand.Instance,
            ClearCommand.PREFIX => ClearCommand.Instance,
            _ => throw new FormatException(),
        };
    }

    /// <summary>
    /// Parses a string representation of some <see cref="ISolverCommand"/>
    /// </summary>
    public static ISolverCommand Parse(string command, TelemetrySource? telemetrySource) {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        if (command.Length == 0)
            throw new FormatException();

        return command[0] switch {
            EditCommand.PREFIX => telemetrySource == null
                               || (telemetrySource.Product == "RDroid"
                                && new Version(telemetrySource.Version) < new Version("0.4.2.48"))
                ? EditCommand.ParseV0(command)
                : EditCommand.Parse(command),
            PlayCommand.PREFIX => PlayCommand.Parse(command),
            PauseCommand.PREFIX => PauseCommand.Instance,
            RedoCommand.PREFIX => RedoCommand.Instance,
            UndoCommand.PREFIX => UndoCommand.Instance,
            StepCommand.PREFIX => StepCommand.Instance,
            StopCommand.PREFIX => StopCommand.Instance,
            _ => throw new FormatException(),
        };
    }
}
#endif