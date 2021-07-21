namespace RepsTabataGenerator

open RepsTabataGenerator.Model

module Configuration =

    type RepeatsAndTimings =
        {
            Warmup: Duration
            WarmupCycles: Option<int>
            Cycles: int
            Work: Duration
            Rest: Duration option
            Recovery: Duration option
            CoolDown: Duration option
        }

    type ExerciseWithReps =
        {
            Name: Label
            BPM: BPM
            GIF: GIF
        }
        static member mk l b g = { Name = l; BPM = b; GIF = g }


    type Exercise =
        | ExerciseDuration of Label
        | ExerciseReps of ExerciseWithReps

    type Workout =
        {
            Id: int
            Title: Label
            Notes: string
            RepeatsAndTimings: RepeatsAndTimings
            ApplicationSettings: Settings
            Exercises: Exercise array
        }
