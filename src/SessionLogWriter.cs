namespace RoboZZle.Telemetry;

using System.Globalization;

using RoboZZle.Telemetry.Actions;

public sealed class SessionLogWriter {
    readonly SessionLog log;

    public SessionLogWriter(SessionLog log) {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        DebugEx.WriteLine("TELEMETRY: session started @ {0}",
                          DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
    }

    PlayCommand? playCommandInProgress;

    public void Log(ISolverCommand command) {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        var entry = new SessionLogEntry {
            Command = command.ToString(),
        };
        this.log.Entries.Add(entry);
        DebugEx.WriteLine("TELEMETRY: {0}", entry);
    }

    public void LogPlayStart(int currentSteps) {
        if (currentSteps < 0)
            throw new ArgumentOutOfRangeException(nameof(currentSteps));

        // negative value indicates incomplete play telemetry
        this.playCommandInProgress = new PlayCommand {
            Steps = -currentSteps,
        };
    }

    public void LogPlayEnd(int currentSteps) {
        if (currentSteps < 0)
            throw new ArgumentOutOfRangeException(nameof(currentSteps));

        if (this.playCommandInProgress == null)
            throw new InvalidOperationException("The last command was not a play command");

        if (this.playCommandInProgress.Steps > 0)
            throw new InvalidOperationException("The last play command has already been stopped");

        if (currentSteps + this.playCommandInProgress.Steps < 0) {
            string errorMessage = string.Format(CultureInfo.InvariantCulture,
                                                "Value must be greater than, or equal to {0}",
                                                -(long)this.playCommandInProgress.Steps);
            throw new ArgumentOutOfRangeException(nameof(currentSteps), currentSteps, errorMessage);
        }

        this.Log(new PlayCommand { Steps = this.playCommandInProgress.Steps + currentSteps });
        this.playCommandInProgress = null;
    }
}