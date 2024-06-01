namespace RoboZZle.Telemetry.Actions;

using System.Collections;
using System.Globalization;

/// <summary>
/// Represents a solution solving command of editing program's specific command slot
/// </summary>
public sealed class EditCommand: ISolverCommand {
    /// <summary>
    /// Only <see cref="EditCommand"/> can start with this prefix
    /// </summary>
    public const char PREFIX = 'E';

    /// <summary>
    /// Index of the function being edited
    /// </summary>
    public int Function { get; init; }
    /// <summary>
    /// Offset of the command, which is being edited, inside a function
    /// </summary>
    public int CommandOffset { get; init; }
    /// <summary>
    /// New command
    /// </summary>
    public Command? NewCommand { get; init; }
    /// <summary>
    /// Command before replacement
    /// </summary>
    public Command? OldCommand { get; init; }

    /// <summary>
    /// Converts this command to its string representation
    /// </summary>
    public override string ToString() {
        string oldCommandString = this.OldCommand.AsString();
        string newCommandString = this.NewCommand.AsString();
        return string.Format(CultureInfo.InvariantCulture,
                             "E{0}{1}:{2}->{3}",
                             this.Function, this.CommandOffset, oldCommandString, newCommandString);
    }

#if !JAVA
    /// <summary>
    /// Parses <see cref="EditCommand"/> from its string representation
    /// </summary>
    public static EditCommand Parse(string editCommand) {
        if (string.IsNullOrEmpty(editCommand))
            throw new ArgumentNullException(nameof(editCommand));

        if (editCommand[0] != PREFIX)
            throw new FormatException($"Input string must begin with '{PREFIX}'");

        if (editCommand.Length != StringLength)
            throw new FormatException("Input string must be of length " + StringLength);

        return new() {
            // TODO: validate
            Function = editCommand[1] - '0',
            // TODO: validate
            CommandOffset = editCommand[2] - '0',
            OldCommand = Command.Parse(editCommand, 4),
            NewCommand = Command.Parse(editCommand, 8),
        };
    }

    /// <summary>
    /// Parses <see cref="EditCommand"/> from its string representation
    /// </summary>
    public static EditCommand ParseV0(string editCommand) {
        if (string.IsNullOrEmpty(editCommand))
            throw new ArgumentNullException(nameof(editCommand));

        if (editCommand[0] != PREFIX)
            throw new FormatException($"Input string must begin with '{PREFIX}'");

        if (editCommand.Length != StringLength)
            throw new FormatException("Input string must be of length " + StringLength);

        return new() {
            // TODO: validate
            Function = editCommand[1] - '0',
            // TODO: validate
            CommandOffset = editCommand[2] - '0',
            OldCommand = Command.Parse(editCommand[4], editCommand[5]),
            NewCommand = Command.Parse(editCommand[8], editCommand[9]),
        };
    }

    static readonly int StringLength = new EditCommand().ToString().Length;

    /// <summary>
    /// Gets hash code for this command
    /// </summary>
    public override int GetHashCode() {
        return this.CommandOffset * 0x25251135 ^ this.Function * 0x2591
                                               ^ StructuralComparisons.StructuralEqualityComparer
                                                     .GetHashCode(this.NewCommand) * 0x1351
                                               ^ StructuralComparisons.StructuralEqualityComparer
                                                     .GetHashCode(this.OldCommand) * 0x1773;
    }

    /// <summary>
    /// Checks if passed object structurally equals to this object.
    /// </summary>
    public override bool Equals(object? obj) {
        var otherEditCommand = obj as EditCommand;
        return otherEditCommand != null
            && otherEditCommand.CommandOffset == this.CommandOffset
            && otherEditCommand.Function == this.Function
            && StructuralComparisons.StructuralEqualityComparer.Equals(
                   otherEditCommand.NewCommand, this.NewCommand)
            && StructuralComparisons.StructuralEqualityComparer.Equals(
                   otherEditCommand.OldCommand, this.OldCommand);
    }

    /// <summary>
    /// Replays command on top of a program editing history
    /// </summary>
    public void Replay(IProgramHistory programHistory) {
        var updatedProgram = programHistory.CurrentProgram.Clone();
        var function = updatedProgram.Functions[this.Function];
        var currentCommand = function.Commands[this.CommandOffset];
        if (!StructuralComparisons.StructuralEqualityComparer.Equals(currentCommand, this.OldCommand))
            throw new InvalidOperationException(
                "Can not replay this command on top of program history: current command at the specified position is different from the expected one");
        function.Commands[this.CommandOffset] = this.NewCommand.Clone();
        programHistory.Add(updatedProgram);
    }
#endif
}