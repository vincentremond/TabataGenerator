using Newtonsoft.Json;
using TabataGenerator.Input;

namespace TabataGenerator.OutputFormat
{
    public class Interval
    {
        public Interval(Duration time, IntervalType type, string description)
        {
            this.time = time.TotalSeconds;
            this.type = type;
            this.description = description;
        }

        public bool addSet => false;
        public int bpm => 0;
        public int cycle => -1;
        public int cyclesCount => -1;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string description { get; }

        public bool isRepsMode => false;
        public int reps => 0;
        public int tabata => -1;
        public int tabatasCount => -1;
        public int time { get; }
        public IntervalType type { get; }
    }
}
