namespace RoboZZle.Telemetry;

using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[TestClass]
public class SolutionSerializationTests {
    [TestMethod]
    public void CanDeserializeVersion() {
        var solutionTelemetry = new SolutionTelemetry {
            StartingProgram = "",
            PuzzleID = 0,
            Source = new TelemetrySource {
                Version = new Version(0, 1).ToString(),
                Product = nameof(SolutionSerializationTests),
            },
        };
        var serializer = new JsonSerializer {
            Converters = { new VersionConverter() },
        };
        using var serializationWriter = new StringWriter();
        serializer.Serialize(serializationWriter, solutionTelemetry);
        string serializedTelemetry = serializationWriter.ToString();
        using var serializationReader = new JsonTextReader(new StringReader(serializedTelemetry));
        var roundTripValue = serializer.Deserialize<SolutionTelemetry>(serializationReader)!;
        Assert.AreEqual(solutionTelemetry.Source, roundTripValue.Source);
    }
}