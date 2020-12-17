using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TabataGenerator.Input;
using TabataGenerator.OutputFormat;

namespace TabataGenerator
{
    internal static class WorkoutConverter
    {
        public static IEnumerable<(string FileName, string Contents)> GenerateWorkoutFiles(string configContent)
        {
            var outputConverter = new OutputConverter();

            var workouts = new WorkoutReader()
                .GetFromContent(configContent)
                .Select(description => (Description: description, Workout: outputConverter.BuildWorkout(description)))
                .ToArray();

            foreach (var (description, workout) in workouts)
            {
                yield return (GetFileName(description), Serialize(new Result(workout)));
            }

            var all = new ResultList(workouts.Select(c => c.Workout).ToArray());
            yield return ("all.workout", Serialize(all));
        }

        private static string GetFileName(WorkoutDescription w) => $"{w.Id} - {w.Label}.workout";

        private static string Serialize<T>(T result)
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
