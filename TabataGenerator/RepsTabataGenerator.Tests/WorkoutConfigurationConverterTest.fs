module RepsTabataGenerator.Tests.WorkoutConfigurationConverterTest

open System
open NUnit.Framework
open RepsTabataGenerator.Configuration
open RepsTabataGenerator.WorkoutConfigurationConverter

[<Test>]
let Test1 () =
    let config =
        {
            Exercises =
                [|
                    {
                        Name = "Ex2"
                        BPM = 180
                        GIF = "https://ex2.gif"
                    }
                |]
            Workouts =
                [|
                    {
                        Id = 1
                        Name = "Title"
                        Notes = "Notes"
                        Template =
                            {
                                Warmup = TimeSpan.FromSeconds(15.)
                                WarmupCycles = Some 2
                                Cycles = 3
                                Work = TimeSpan.FromSeconds(30.)
                                Rest = TimeSpan.FromSeconds(5.)
                                Recovery = TimeSpan.FromSeconds(60.)
                                CoolDown = TimeSpan.FromSeconds(300.)
                            }
                        Settings = None
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
                Settings = None
                Exercises =
                    [|
                        Exercise.ExerciseDuration("Ex1")
                        Exercise.ExerciseReps("Ex2", 180, "https://ex2.gif")
                    |]
            }
        |]

    let actual = convertConfig config

    Assert.AreEqual(expected, actual)
    ()
