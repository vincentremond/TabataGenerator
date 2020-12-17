using System;
using System.IO;
using System.Linq;

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
            var generateWorkoutFiles = WorkoutConverter.GenerateWorkoutFiles(configContent);
            foreach (var (fileName, contents) in generateWorkoutFiles)
            {
                Console.WriteLine($"Writing to file : {fileName}");
                WriteToFile(resultDirectory, fileName, contents);
            }

            Console.WriteLine("done");
        }

        private static void WriteToFile(string resultDirectory, string fileName, string contents)
        {
            var outputPath = Path.Combine(resultDirectory, fileName);
            File.WriteAllText(outputPath, contents);
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
    }
}
