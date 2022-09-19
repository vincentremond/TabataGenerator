module RepsTabataGenerator.Tests.WorkoutIntervalExpanderTest

open FsUnit
open NUnit.Framework
open RepsTabataGenerator.Configuration
open RepsTabataGenerator.Model
open RepsTabataGenerator.WorkoutIntervalExpander

[<Test>]
let Test1 () =
    {
        Id = 1
        Title = "Title"
        Notes = "Notes"
        RepeatsAndTimings =
            {
                Warmup = 15.<sec>
                Work = 30.<sec>
                Rest = 5.<sec> |> Some
                Recovery = 60.<sec> |> Some
                CoolDown = 300.<sec> |> Some
                WarmupCycles = Some 2
                Cycles = 3
                Pace = None
            }
        ApplicationSettings = None
        Exercises =
            [|
                Exercise.ExerciseDuration("Ex1")
                Exercise.ExerciseReps(ExerciseWithReps.mk "Ex2" 180.<reps/min> "https://ex2.gif")
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
                    DetailedInterval.WorkReps("Warmup\n[Ex. 2/2 • Cycle 1/2+3]\nEx2", 72.<reps>, 144.<reps/min>)
                    DetailedInterval.Recovery(60.<sec>)
                    DetailedInterval.WorkDuration("Warmup\n[Ex. 1/2 • Cycle 2/2+3]\nEx1", 30.<sec>)
                    DetailedInterval.Rest(5.<sec>)
                    DetailedInterval.WorkReps("Warmup\n[Ex. 2/2 • Cycle 2/2+3]\nEx2", 72.<reps>, 144.<reps/min>)
                    DetailedInterval.Recovery(60.<sec>)
                    DetailedInterval.WorkDuration("\n[Ex. 1/2 • Cycle 1/3]\nEx1", 30.<sec>)
                    DetailedInterval.Rest(5.<sec>)
                    DetailedInterval.WorkReps("\n[Ex. 2/2 • Cycle 1/3]\nEx2", 90.<reps>, 180.<reps/min>)
                    DetailedInterval.Recovery(60.<sec>)
                    DetailedInterval.WorkDuration("\n[Ex. 1/2 • Cycle 2/3]\nEx1", 30.<sec>)
                    DetailedInterval.Rest(5.<sec>)
                    DetailedInterval.WorkReps("\n[Ex. 2/2 • Cycle 2/3]\nEx2", 90.<reps>, 180.<reps/min>)
                    DetailedInterval.Recovery(60.<sec>)
                    DetailedInterval.WorkDuration("\n[Ex. 1/2 • Cycle 3/3]\nEx1", 30.<sec>)
                    DetailedInterval.Rest(5.<sec>)
                    DetailedInterval.WorkReps("\n[Ex. 2/2 • Cycle 3/3]\nEx2", 90.<reps>, 180.<reps/min>)
                    DetailedInterval.CoolDown(300.<sec>)
                |]
        }
