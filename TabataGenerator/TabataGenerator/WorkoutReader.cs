using TabataGenerator.Input;
using YamlDotNet.Serialization;

namespace TabataGenerator
{
    public class WorkoutReader
    {
        public WorkoutDescription[] Read(string input)
        {
            return new Deserializer().Deserialize<WorkoutDescription[]>(input);
        }
    }
}
