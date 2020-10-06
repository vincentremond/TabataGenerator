using System;

namespace TabataGenerator.Input
{
    [Serializable]
    public class WorkoutDescription
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public Duration Warmup { get; set; }
        public int WarmupCycles { get; set; }
        public int Cycles { get; set; }
        public Duration Work { get; set; }
        public Duration Rest { get; set; }
        public Duration Recovery { get; set; }
        public Duration CoolDown { get; set; }
        public string[] Exercises { get; set; }

        public WorkoutDescription()
            : this(id: 0,
                label: "(no label)",
                warmup: Duration.Empty,
                warmupCycles: 0,
                cycles: 1,
                work: Duration.FromSeconds(30),
                rest: Duration.FromSeconds(30),
                recovery: Duration.Empty,
                coolDown: Duration.Empty,
                exercises: new[]
                {
                    "(no label)"
                }
            )
        {
        }

        public WorkoutDescription(
            int id,
            string label,
            Duration warmup,
            int warmupCycles,
            int cycles,
            Duration work,
            Duration rest,
            Duration recovery,
            Duration coolDown,
            string[] exercises)
        {
            Id = id;
            Label = label;
            Warmup = warmup;
            WarmupCycles = warmupCycles;
            Cycles = cycles;
            Work = work;
            Rest = rest;
            Recovery = recovery;
            CoolDown = coolDown;
            Exercises = exercises;
        }
    }
}
