using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TabataGenerator.OutputFormat;

namespace TabataGenerator
{
    class Program
    {
        private const string ExercicesFile = @"exercises.yml";

        static void Main()
        {
            var configFile = GetConfigFilePathInfo();
            var resultDirectory = GetResultDirectoryPath(configFile);
            var configContent = File.ReadAllText(configFile.FullName);
            var workouts = new WorkoutReader().Read(configContent);

            var outputConverter = new OutputConverter();
            foreach (var workout in workouts)
            {
                var result = outputConverter.BuildResult(workout);
                var serializedObject = Serialize(result);
                Console.WriteLine(result.Workout.Title);
                WriteToFile(serializedObject, result.Workout.Title, resultDirectory);
            }

            Console.WriteLine("done");
        }

        private static string GetResultDirectoryPath(FileInfo configFile)
        {
            var resultDirectory = Path.Combine(configFile.Directory.FullName, "Results");
            if (!Directory.Exists(resultDirectory))
            {
                Directory.CreateDirectory(resultDirectory);
            }

            return resultDirectory;
        }

        private static FileInfo GetConfigFilePathInfo()
        {
            var currentDirectory = Environment.CurrentDirectory;
            var fi = new DirectoryInfo(currentDirectory);
            while (fi != null)
            {
                var file = fi.GetFiles(ExercicesFile, SearchOption.TopDirectoryOnly).SingleOrDefault();
                if (file != null)
                {
                    return file;
                }

                fi = fi.Parent;
            }

            throw new Exception($"Config file {ExercicesFile} not found.");
        }

        private static void WriteToFile(string serializedObject, string workoutLabel, string resultDirectory)
        {
            var outputPath = Path.Combine(resultDirectory, $"{workoutLabel}.workout");
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
