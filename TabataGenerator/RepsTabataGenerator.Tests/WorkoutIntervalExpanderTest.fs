module RepsTabataGenerator.Tests.WorkoutIntervalExpanderTest

open FsUnit
open NUnit.Framework
open RepsTabataGenerator.Model
open RepsTabataGenerator.WorkoutConfigurationConverter
open RepsTabataGenerator.WorkoutIntervalExpander

[<Test>]
let Test1 () =
    {
        Id = 1
        Title = "Title"
        Notes = "Notes"
        Warmup = 15.<sec>
        Work = 30.<sec>
        Rest = 5.<sec>
        Recovery = 60.<sec>
        CoolDown = Some 300.<sec>
        WarmupCycles = Some 2
        Cycles = 3
        Settings = None
        Exercises =
            [|
                Exercise.ExerciseDuration("Ex1")
                Exercise.ExerciseReps("Ex2", 180.<reps/min>, "https://ex2.gif")
            |]
    }
    |> create
    |> should
        equal
           {
               Id = 1
               Title = "Title"
               Notes = "Notes\n\nExercises:\n• Ex1\n• Ex2"
               Settings = None
               Intervals =
                   [|
                       DetailedInterval.Prepare(15.<sec>)
                       DetailedInterval.WorkDuration("Warmup\n[Ex. 1/2 • Cycle 1/2+3]\nEx1", 30.<sec>)
                       DetailedInterval.Rest(5.<sec>)
                       DetailedInterval.WorkReps("Warmup\n[Ex. 2/2 • Cycle 1/2+3]\nEx2", 68.<reps>, 135.<reps/min>, "https://ex2.gif")
                       DetailedInterval.Recovery(60.<sec>)
                       DetailedInterval.WorkDuration("Warmup\n[Ex. 1/2 • Cycle 2/2+3]\nEx1", 30.<sec>)
                       DetailedInterval.Rest(5.<sec>)
                       DetailedInterval.WorkReps("Warmup\n[Ex. 2/2 • Cycle 2/2+3]\nEx2", 68.<reps>, 135.<reps/min>, "https://ex2.gif")
                       DetailedInterval.Recovery(60.<sec>)
                       DetailedInterval.WorkDuration("\n[Ex. 1/2 • Cycle 1/3]\nEx1", 30.<sec>)
                       DetailedInterval.Rest(5.<sec>)
                       DetailedInterval.WorkReps("\n[Ex. 2/2 • Cycle 1/3]\nEx2", 90.<reps>, 180.<reps/min>, "https://ex2.gif")
                       DetailedInterval.Recovery(60.<sec>)
                       DetailedInterval.WorkDuration("\n[Ex. 1/2 • Cycle 2/3]\nEx1", 30.<sec>)
                       DetailedInterval.Rest(5.<sec>)
                       DetailedInterval.WorkReps("\n[Ex. 2/2 • Cycle 2/3]\nEx2", 90.<reps>, 180.<reps/min>, "https://ex2.gif")
                       DetailedInterval.Recovery(60.<sec>)
                       DetailedInterval.WorkDuration("\n[Ex. 1/2 • Cycle 3/3]\nEx1", 30.<sec>)
                       DetailedInterval.Rest(5.<sec>)
                       DetailedInterval.WorkReps("\n[Ex. 2/2 • Cycle 3/3]\nEx2", 90.<reps>, 180.<reps/min>, "https://ex2.gif")
                       DetailedInterval.CoolDown(300.<sec>)
                   |]
           }
