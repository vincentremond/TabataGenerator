namespace TabataGenerator.JsonModels
{
    public class Interval
    {
        public Interval(int bpm, bool isRepsMode, int reps, int time, int type, string? description, string? url)
        {
            Bpm = bpm;
            IsRepsMode = isRepsMode;
            Reps = reps;
            Time = time;
            Type = type;
            Description = description;
            Url = url;
        }
        
        public bool AddSet => false;
        public int Bpm { get; }
        public int Cycle => -1;
        public int CyclesCount => -1;
        public bool IsRepsMode { get; }
        public int Reps { get; }
        public int Tabata => -1;
        public int TabatasCount => -1;
        public int Time { get; }
        public int Type { get; }
        public string? Description { get; }
        public string? Url { get; }
    }
}
