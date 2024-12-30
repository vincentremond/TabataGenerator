namespace RepsTabataGenerator

open System
open Configuration
open RepsTabataGenerator
open RepsTabataGenerator.Model

module EntryPoint =

    let writeToFile path (contents: string) =
        System.IO.File.WriteAllText(path, contents)

    [<EntryPoint>]
    let main _ =

        let defaultSpinnerGif = "https://media.giphy.com/media/2WjpfxAI5MvC9Nl8U7/giphy.gif"

        let exercises = [|
            ExerciseWithReps.mk "Bear plank" 42.<reps / min>
            ExerciseWithReps.mk "Break dancer" 23.<reps / min>
            ExerciseWithReps.mk "Burpees" 13.<reps / min>
            ExerciseWithReps.mk "Jumping jack" 125.<reps / min>
            ExerciseWithReps.mk "Montées de genoux" 140.<reps / min>
            ExerciseWithReps.mk "Mountain climbers" 100.<reps / min>
            ExerciseWithReps.mk "Pompes en T" 13.<reps / min>
            ExerciseWithReps.mk "Skater" 40.<reps / min>
            ExerciseWithReps.mk "Squat foot touch" 23.<reps / min>
            ExerciseWithReps.mk "Squats sautés" 40.<reps / min>
            ExerciseWithReps.mk "Step up" 30.<reps / min>
            ExerciseWithReps.mk "Coude genoux" 30.<reps / min>
        |]

        let asEx input =
            let findEx s =
                match (exercises |> Seq.filter (fun e -> e.Name = s) |> Seq.tryExactlyOne) with
                | Some e -> ExerciseReps e
                | None -> ExerciseDuration s

            input |> Array.map findEx

        let hiitMachine = {
            Warmup = (7.<min> |%| 30.<sec>)
            WarmupCycles = None
            Cycles = 16
            Work = 30.<sec>
            Rest = 30.<sec> |> Some
            Recovery = 30.<sec> |> Some
            CoolDown = 30.<sec> |> Some
            Pace = None
        }

        let hiit = {
            Warmup = 20.<sec>
            WarmupCycles = Some 2
            Cycles = 5
            Work = 25.<sec>
            Rest = 35.<sec> |> Some
            Recovery = 35.<sec> |> Some
            CoolDown = (3.<min> |%| 0.<sec>) |> Some
            Pace = None
        }

        let circuitTraining = {
            Warmup = 20.<sec>
            WarmupCycles = Some 2
            Cycles = 5
            Work = 30.<sec>
            Rest = 4.<sec> |> Some
            Recovery = (1.<min> |%| 15.<sec>) |> Some
            CoolDown = (3.<min> |%| 0.<sec>) |> Some
            Pace = None
        }

        [|
            {
                Id = 101
                Title = "Poids du corps 1 (new)"
                Notes = "https://90daylc.thibaultgeoffray.com/mes-routines/phase-1/routine-38"
                RepeatsAndTimings = hiit
                ApplicationSettings = None
                Exercises =
                    [|
                        "Squat foot touch"
                        "Montées de genoux"
                        "Pompes en T"
                        "Burpees"
                    |]
                    |> asEx
            }
            {
                Id = 102
                Title = "Poids du corps 4 (new)"
                Notes = "https://90daylc.thibaultgeoffray.com/mes-routines/phase-1/routine-49"
                RepeatsAndTimings = { hiit with Cycles = 4 }
                ApplicationSettings = None
                Exercises =
                    [|
                        "Skater"
                        "Bear plank"
                        "Step up"
                        "Break dancer"
                        "Step up"
                    |]
                    |> asEx
            }
            {
                Id = 103
                Title = "Poids du corps 2 (old)"
                Notes = "https://90daylc.thibaultgeoffray.com/mes-routines/phase-1/routine-12"
                RepeatsAndTimings = hiit
                ApplicationSettings = None
                Exercises =
                    [|
                        "Squats sautés"
                        "Montées de genoux"
                        "Mountain climbers"
                        "Jumping jack"
                    |]
                    |> asEx
            }
            {
                Id = 150
                Title = "Course"
                Notes = ""
                RepeatsAndTimings = hiitMachine
                ApplicationSettings = Some [| "key_workout_sound_cool_down", "value_sound_woohoo" |]
                Exercises = [| "Effort max" |] |> asEx
            }
            {
                Id = 151
                Title = "Vélo elliptique"
                Notes = ""
                RepeatsAndTimings = {
                    Warmup = (7.<min> |%| 30.<sec>)
                    WarmupCycles = None
                    Cycles = 16
                    Work = 30.<sec>
                    Rest = None
                    Recovery = None
                    CoolDown = (5.<min> |%| 0.<sec>) |> Some
                    Pace =
                        Some {
                            Work = 110.<reps / min>
                            Rest = 90.<reps / min>
                        }
                }
                ApplicationSettings =
                    Some [|
                        "key_workout_sound_cool_down", "value_sound_woohoo"
                        "key_workout_sound_metronome_volume_percent", "60"
                    |]
                Exercises = [| "Work" |] |> asEx
            }
            {
                Id = 152
                Title = "Planche"
                Notes = ""
                RepeatsAndTimings = {
                    Warmup = 10.<sec>
                    WarmupCycles = None
                    Cycles = 2
                    Work = 60.<sec>
                    Rest = 45.<sec> |> Some
                    Recovery = 45.<sec> |> Some
                    CoolDown = None
                    Pace = None
                }
                ApplicationSettings = Some [| "key_workout_sound_cool_down", "value_sound_woohoo" |]
                Exercises = [| "Hold" |] |> asEx
            }
            {
                Id = 200
                Title = "Vacuum"
                Notes = ""
                ApplicationSettings =
                    Some [|
                        ("key_workout_sound_time", "12")
                        ("key_workout_sound_latest_seconds", "value_sound_wood_2")
                        ("key_workout_sound_last_seconds_work", "value_sound_glass")
                    |]
                RepeatsAndTimings = {
                    Warmup = 42.<sec>
                    WarmupCycles = None
                    Cycles = 6
                    Work = 30.<sec>
                    Rest = 25.<sec> |> Some
                    Recovery = 25.<sec> |> Some
                    CoolDown = None
                    Pace = None
                }
                Exercises = [| "Hold" |] |> asEx
            }
            {
                Id = 300
                Title = "Méditation"
                Notes = ""
                ApplicationSettings = None
                RepeatsAndTimings = {
                    Warmup = 10.<sec>
                    WarmupCycles = None
                    Cycles = 1
                    Work = (15.<min> |%| 30.<sec>)
                    Rest = 10.<sec> |> Some
                    Recovery = 10.<sec> |> Some
                    CoolDown = 10.<sec> |> Some
                    Pace = None
                }
                Exercises = [| "~~~~" |] |> asEx
            }
        |]
        |> Array.map WorkoutIntervalExpander.create
        |> OutputFileFormat.createResult
        |> OutputFileFormat.serialize
        |> writeToFile "result.workout"

        printfn "Done, written to file"
        0 // return an integer exit code
