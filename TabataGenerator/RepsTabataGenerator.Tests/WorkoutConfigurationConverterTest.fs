module RepsTabataGenerator.Tests.WorkoutConfigurationConverterTest

open System
open NUnit.Framework
open RepsTabataGenerator.Configuration
open RepsTabataGenerator.WorkoutConfigurationConverter

[<Test>]
let Test1 () =
    let xx =
        {
            Exercises =
                [|
                    {
                        Name = "Ex2"
                        BPM = 180
                        GIF = "https://ex2.gif"
                    }
                |]
            Templates =
                [|
                    {
                        Name = "Template1"
                        Warmup = TimeSpan.FromSeconds(15.)
                        WarmupCycles = Some 2
                        Cycles = 3
                        Work = TimeSpan.FromSeconds(30.)
                        Rest = TimeSpan.FromSeconds(5.)
                        Recovery = TimeSpan.FromSeconds(60.)
                        CoolDown = TimeSpan.FromSeconds(300.)
                    }
                |]
            Workouts =
                [|
                    {
                        Id = 1
                        Name = "Title"
                        Notes = "Notes"
                        Template = "Template1"
                        WarmupCycles = None
                        Cycles = Some 3
                        Exercises = [| "Ex1"; "Ex2" |]
                    }
                |]

        }

    let expected =
        [|
            {
                Id = 1
                Title = "Title"
                Notes = "Notes"
                Warmup = TimeSpan.FromSeconds(15.)
                Work = TimeSpan.FromSeconds(30.)
                Rest = TimeSpan.FromSeconds(5.)
                Recovery = TimeSpan.FromSeconds(60.)
                CoolDown = TimeSpan.FromSeconds(300.)
                WarmupCycles = Some 2
                Cycles = 3
                Exercises =
                    [|
                        Exercise.ExerciseDuration("Ex1")
                        Exercise.ExerciseReps("Ex2", 180, "https://ex2.gif")
                    |]
            }
        |]

    let actual = convertConfig xx

    Assert.AreEqual(expected, actual)
    ()
