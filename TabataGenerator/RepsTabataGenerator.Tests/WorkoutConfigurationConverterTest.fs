﻿module RepsTabataGenerator.Tests.WorkoutConfigurationConverterTest

open NUnit.Framework
open RepsTabataGenerator.Model
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
                        BPM = 180.<reps/min>
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
                                Warmup = 15.<sec>
                                WarmupCycles = Some 2
                                Cycles = 3
                                Work = 30.<sec>
                                Rest = 5.<sec>
                                Recovery = 60.<sec>
                                CoolDown = 300.<sec>
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
                Warmup = 15.<sec>
                Work = 30.<sec>
                Rest = 5.<sec>
                Recovery = 60.<sec>
                CoolDown = 300.<sec>
                WarmupCycles = Some 2
                Cycles = 3
                Settings = None
                Exercises =
                    [|
                        Exercise.ExerciseDuration("Ex1")
                        Exercise.ExerciseReps("Ex2", 180.<reps/min>, "https://ex2.gif")
                    |]
            }
        |]

    let actual = convertConfig config

    Assert.AreEqual(expected, actual)
    ()
