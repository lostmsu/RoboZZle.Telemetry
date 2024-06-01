namespace RoboZZle.Telemetry.Actions;

using System.Globalization;

/// <summary>
/// Represents a solution solving command of having program executing deterministic number of steps.
/// </summary>
public sealed class PlayCommand: ISolverCommand {
    /// <summary>
    /// Only <see cref="PlayCommand"/> can start with this prefix in a string representation
    /// </summary>
    public const char PREFIX = '>';

    /// <summary>
    /// Number of 
    /// </summary>
    public int Steps { get; init; }

    public PlayCommand WithSteps(int steps) => new() {
        Steps = steps,
    };

    /// <summary>
    /// Converts this command to its string representation
    /// </summary>
    public override string ToString() {
        return string.Format(CultureInfo.InvariantCulture, ">{0}", this.Steps);
    }

    /// <summary>
    /// Parses <see cref="PlayCommand"/> from its string representation
    /// </summary>
    public static PlayCommand Parse(string command) {
        if (string.IsNullOrEmpty(command))
            throw new ArgumentNullException(nameof(command));

        int steps = int.Parse(command.Substring(1), CultureInfo.InvariantCulture);
        return new() {
            Steps = steps,
        };
    }

    /// <summary>
    /// Gets hash code for this command
    /// </summary>
    public override int GetHashCode() {
        return this.Steps;
    }

    /// <summary>
    /// Checks if passed object structurally equals to this object.
    /// </summary>
    public override bool Equals(object? obj) {
        var otherPlayCommand = obj as PlayCommand;
        return otherPlayCommand != null && otherPlayCommand.Steps == this.Steps;
    }
#if !JAVA
    public void Replay(IProgramHistory programHistory) {
        // has no effect on program editing history
    }
#endif
}