namespace RepsTabataGenerator

open RepsTabataGenerator.Model

module Configuration =

    type Template =
        {
            Warmup: Duration
            WarmupCycles: Option<int>
            Cycles: int
            Work: Duration
            Rest: Duration
            Recovery: Duration
            CoolDown: Duration option
        }

    type Exercise = { Name: Label; BPM: BPM; GIF: GIF }

    type Workout =
        {
            Id: int
            Name: Label
            Notes: string
            Template: Template
            Settings: Settings
            Exercises: string array
        }

    type Config =
        {
            Exercises: Exercise array
            Workouts: Workout array
        }
