using System;
using Newtonsoft.Json;
using TabataGenerator.Input;

namespace TabataGenerator.OutputFormat
{
    [Serializable]
    public class Interval
    {
        public Interval(Duration time, IntervalType type, string description)
        {
            Time = time.TotalSeconds;
            Type = type;
            Description = description;
        }

        public bool AddSet => false;
        public int Bpm => 0;
        public int Cycle => -1;
        public int CyclesCount => -1;
        public bool IsRepsMode => false;
        public int Reps => 0;
        public int Tabata => -1;
        public int TabatasCount => -1;
        public int Time { get; }
        public IntervalType Type { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; }
    }
}
