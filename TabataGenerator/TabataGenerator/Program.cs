using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TabataGenerator.OutputFormat;

namespace TabataGenerator
{
    class Program
    {
        // TODO VRM add config ?!
        private const string ExercicesFile = @"D:\GoogleDrive\GitRepositories\TabataGenerator\exercises.yml";
        private const string ResultDirectory = @"D:\GoogleDrive\GitRepositories\TabataGenerator\results\";

        static void Main()
        {
            var contents = File.ReadAllText(ExercicesFile);
            var workouts = new WorkoutReader().Read(contents);

            var outputConverter = new OutputConverter();
            foreach (var workout in workouts)
            {
                var result = outputConverter.BuildResult(workout);
                var serializedObject = Serialize(result);
                WriteToFile(serializedObject, workout.Label);
            }
        }

        private static void WriteToFile(string serializedObject, string workoutLabel)
        {
            var outputPath = Path.Combine(ResultDirectory, $"{workoutLabel}.workout");
            File.WriteAllText(outputPath, serializedObject);
        }

        private static string Serialize(Result result)
        {
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };
            jsonSerializerSettings.Converters.Add(new MyConverter());
            var serializedObject = JsonConvert.SerializeObject(result, jsonSerializerSettings);
            return serializedObject;
        }
    }
}
