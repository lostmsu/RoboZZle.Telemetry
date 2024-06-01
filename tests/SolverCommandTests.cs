namespace RoboZZle.Telemetry;

using System.Reflection;

using RoboZZle.Telemetry.Actions;

[TestClass]
public class SolverCommandTests {
    [TestMethod]
    public void SingletonCommandsParsedCorrectly() {
        var singletonCommands = GetCommandTypes().Where(IsSingleton).ToArray();
        Assert.AreNotEqual(0, singletonCommands.Length, "no singleton commands found");
        foreach (var commandType in singletonCommands) {
            object instance = GetInstanceProperty(commandType)!.GetValue(null)!;
            string serialized = instance.ToString()!;
            try {
                var deserialized = SolverCommand.Parse(serialized);
                Assert.AreSame(instance, deserialized);
            } catch (FormatException) {
                Assert.Fail($"failed to deserialize {instance.GetType().Name}");
            }
        }
    }

    [TestMethod]
    public void EditCommandSerializationRoundtrip() {
        CommandSerializationRoundtrip(new EditCommand {
            CommandOffset = 1,
            Function = 2,
            NewCommand = new Command {
                Action = new Movement { Kind = MovementKind.TURN_LEFT },
                Condition = Color.GREEN,
            },
            OldCommand = new Command {
                Action = new Call { Function = 1 },
                Condition = null,
            },
        });
    }

    [TestMethod]
    public void PlayCommandSerializationRoundtrip() {
        CommandSerializationRoundtrip(new PlayCommand { Steps = 42 });
    }

    [TestMethod]
    public void EnsureAllNonTrivialSerializationRoundtripsAreTested() {
        var nonTrivialCommands = GetCommandTypes().Where(t => !IsSingleton(t)).ToArray();
        Assert.AreNotEqual(0, nonTrivialCommands.Length, "no non-trivial commands found");
        var coveredCommands = typeof(SolverCommandTests).GetMethods()
                                                        .Where(m => m.Name.EndsWith(
                                                                   "SerializationRoundtrip",
                                                                   StringComparison.Ordinal))
                                                        .Select(test => test.Name.Substring(
                                                                    0,
                                                                    test.Name.Length -
                                                                    "SerializationRoundtrip"
                                                                        .Length));
        string[] uncoveredCommands =
            nonTrivialCommands.Select(command => command.Name).Except(coveredCommands).ToArray();
        Assert.AreEqual(0, uncoveredCommands.Length, string.Join(", ", uncoveredCommands));
    }

    static void CommandSerializationRoundtrip(ISolverCommand originalCommand) {
        string serialized = originalCommand.ToString();
        var deserialized = SolverCommand.Parse(serialized);
        Assert.AreEqual(originalCommand, deserialized);
    }

    static IEnumerable<Type> GetCommandTypes() =>
        typeof(SolverCommand).GetTypeInfo().Assembly.GetTypes()
                             .Where(t => t.GetTypeInfo()
                                          .GetInterface(typeof(ISolverCommand).FullName!) != null);

    static PropertyInfo? GetInstanceProperty(Type commandType) =>
        commandType.GetProperty("Instance", BindingFlags.Static | BindingFlags.Public);

    static bool IsSingleton(Type commandType) => GetInstanceProperty(commandType) != null;
}