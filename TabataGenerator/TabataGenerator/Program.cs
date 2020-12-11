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
                var fileName = GetFileName(workout.Id, workout.Label);
                var result = outputConverter.BuildResult(workout);
                var serializedObject = Serialize(result);
                Console.WriteLine(fileName);
                WriteToFile(serializedObject, resultDirectory, fileName);
            }

            Console.WriteLine("done");
        }

        private static string GetFileName(int id, string title)
        {
            return $"{id} - {title}.workout";
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

        private static void WriteToFile(string serializedObject, string resultDirectory, string fileName)
        {
            var outputPath = Path.Combine(resultDirectory, fileName);
            File.WriteAllText(outputPath, serializedObject);
        }

        private static string Serialize(Result result)
        {
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };
            jsonSerializerSettings.Converters.Add(new MyJsonConverter());
            var serializedObject = JsonConvert.SerializeObject(result, jsonSerializerSettings);
            return serializedObject;
        }
    }
}
