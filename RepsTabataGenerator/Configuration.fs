namespace RepsTabataGenerator

open RepsTabataGenerator.Model

module Configuration =

    type RepeatsAndTimings = {
        Warmup: Duration
        WarmupCycles: Option<int>
        Cycles: int
        Work: Duration
        Rest: Duration option
        Recovery: Duration option
        CoolDown: Duration option
        Pace: Pace option
    }

    type ExerciseWithReps = {
        Name: Label
        BPM: BPM
    } with

        static member mk l b = {
            Name = l
            BPM = b
        }

    type Exercise =
        | ExerciseDuration of Label
        | ExerciseReps of ExerciseWithReps

    type Workout = {
        Id: int
        Title: Label
        Notes: string
        RepeatsAndTimings: RepeatsAndTimings
        ApplicationSettings: Settings
        Exercises: Exercise array
    }
