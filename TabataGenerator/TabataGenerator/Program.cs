using System.IO;
using Newtonsoft.Json;

namespace TabataGenerator
{
    class Program
    {
        // TODO VRM add config ?!
        private const string _workingDirectory = @"D:\VRM\Projects\TabataGenerator";

        static void Main()
        {
            var path = Path.Combine(_workingDirectory, "exercises.yml");
            var contents = File.ReadAllText(path);
            var workouts = new WorkoutReader().Read(contents);

            var outputConverter = new OutputConverter();
            foreach (var workout in workouts)
            {
                var result = outputConverter.BuildResult(workout);
                var serializedObject = JsonConvert.SerializeObject(result, Formatting.None);
                var outputPath = Path.Combine(_workingDirectory, "results", $"{workout.Label}.workout");
                File.WriteAllText(outputPath, serializedObject);
            }
        }
    }
}
