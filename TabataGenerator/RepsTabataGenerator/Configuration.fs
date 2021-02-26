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
    
    type Exercise =
        | ExerciseDuration of Label
        | ExerciseReps of Label * BPM * GIF

    type Workout =
        {
            Id: int
            Title: Label
            Notes: string
            Template: Template
            Settings: Settings
            Exercises: Exercise array
        }
