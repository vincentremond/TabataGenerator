module RepsTabataGenerator.Tests.WorkoutIntervalExpanderTest

open System
open NUnit.Framework
open RepsTabataGenerator.WorkoutConfigurationConverter
open RepsTabataGenerator.WorkoutIntervalExpander

[<Test>]
let Test1 () =
    let payload =
        {
            Id = 1
            Title = "Title"
            Notes = "Notes"
            Warmup = TimeSpan.FromSeconds(15.)
            Work = TimeSpan.FromSeconds(30.)
            Rest = TimeSpan.FromSeconds(5.)
            Recovery = TimeSpan.FromSeconds(60.)
            CoolDown =  TimeSpan.FromSeconds(300.)
            WarmupCycles = Some 2
            Cycles = 3
            Exercises =
                [|
                    Exercise.ExerciseDuration("Ex1")
                    Exercise.ExerciseReps("Ex2", 180, "https://ex2.gif")
                |]
        }

    let expected =
        {
            Id = 1
            Title = "Title"
            Notes = "Notes"
            Intervals =
                [|
                    DetailedInterval.Prepare(TimeSpan.FromSeconds(15.))
                    DetailedInterval.WorkDuration("Warmup\n[Ex. 1/2 • Cycle 1/2]\nEx1", TimeSpan.FromSeconds(30.))
                    DetailedInterval.Rest(TimeSpan.FromSeconds(5.))
                    DetailedInterval.WorkReps("Warmup\n[Ex. 2/2 • Cycle 1/2]\nEx2", 68, 180, "https://ex2.gif")
                    DetailedInterval.Recovery(TimeSpan.FromSeconds(60.))
                    DetailedInterval.WorkDuration("Warmup\n[Ex. 1/2 • Cycle 2/2]\nEx1", TimeSpan.FromSeconds(30.))
                    DetailedInterval.Rest(TimeSpan.FromSeconds(5.))
                    DetailedInterval.WorkReps("Warmup\n[Ex. 2/2 • Cycle 2/2]\nEx2", 68, 180, "https://ex2.gif")
                    DetailedInterval.Recovery(TimeSpan.FromSeconds(60.))
                    DetailedInterval.WorkDuration("\n[Ex. 1/2 • Cycle 1/3]\nEx1", TimeSpan.FromSeconds(30.))
                    DetailedInterval.Rest(TimeSpan.FromSeconds(5.))
                    DetailedInterval.WorkReps("\n[Ex. 2/2 • Cycle 1/3]\nEx2", 90, 180, "https://ex2.gif")
                    DetailedInterval.Recovery(TimeSpan.FromSeconds(60.))
                    DetailedInterval.WorkDuration("\n[Ex. 1/2 • Cycle 2/3]\nEx1", TimeSpan.FromSeconds(30.))
                    DetailedInterval.Rest(TimeSpan.FromSeconds(5.))
                    DetailedInterval.WorkReps("\n[Ex. 2/2 • Cycle 2/3]\nEx2", 90, 180, "https://ex2.gif")
                    DetailedInterval.Recovery(TimeSpan.FromSeconds(60.))
                    DetailedInterval.WorkDuration("\n[Ex. 1/2 • Cycle 3/3]\nEx1", TimeSpan.FromSeconds(30.))
                    DetailedInterval.Rest(TimeSpan.FromSeconds(5.))
                    DetailedInterval.WorkReps("\n[Ex. 2/2 • Cycle 3/3]\nEx2", 90, 180, "https://ex2.gif")
                    DetailedInterval.CoolDown(TimeSpan.FromSeconds(300.))
                |]
        }


    let actual = create payload
    printf "%A" actual

    Assert.AreEqual(expected, actual)
