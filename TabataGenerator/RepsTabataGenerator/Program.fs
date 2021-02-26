namespace RepsTabataGenerator

open System
open Configuration
open RepsTabataGenerator
open RepsTabataGenerator.Model

module EntryPoint =

    let writeToFile path contents =
        System.IO.File.WriteAllText(path, contents)

    let hiitMachine =
        {
            Warmup = duration (7.<min> |%| 30.<sec>)
            WarmupCycles = Some 0
            Cycles = 16
            Work = duration 30.<sec>
            Rest = duration 30.<sec>
            Recovery = duration 30.<sec>
            CoolDown = Some (duration 30.<sec>)
        }

    let hiit =
        {
            Warmup = duration 20.<sec>
            WarmupCycles = Some 2
            Cycles = 5
            Work = duration 25.<sec>
            Rest = duration 35.<sec>
            Recovery = duration 35.<sec>
            CoolDown = Some (duration (3.<min> |%| 0.<sec>))
        }

    let circuitTraining =
        {
            Warmup = duration 20.<sec>
            WarmupCycles = Some 2
            Cycles = 5
            Work = duration 30.<sec>
            Rest = duration 4.<sec>
            Recovery = duration (1.<min> |%| 15.<sec>)
            CoolDown = Some (duration (3.<min> |%| 0.<sec>))
        }

    let config =
        {
            Exercises =
                [|
                    { Name = "Bear plank" ; BPM = 42.<reps/min> ; GIF = "https://media.giphy.com/media/2WjpfxAI5MvC9Nl8U7/giphy.gif" }
                    { Name = "Break dancer" ; BPM = 23.<reps/min> ; GIF = "https://media.giphy.com/media/2WjpfxAI5MvC9Nl8U7/giphy.gif" }
                    { Name = "Burpees" ; BPM = 17.<reps/min> ; GIF = "https://pas-bien.net/divers/tabata/burpees.gif" }
                    { Name = "Jumping jack" ; BPM = 120.<reps/min> ; GIF = "https://media.giphy.com/media/2WjpfxAI5MvC9Nl8U7/giphy.gif" }
                    { Name = "Montées de genoux" ; BPM = 135.<reps/min> ; GIF = "https://pas-bien.net/divers/tabata/montees-genoux.gif" }
                    { Name = "Mountain climbers" ; BPM = 100.<reps/min> ; GIF = "https://pas-bien.net/divers/tabata/montain-climbers.gif" }
                    { Name = "Pompes en T" ; BPM = 12.<reps/min> ; GIF = "https://media.giphy.com/media/2WjpfxAI5MvC9Nl8U7/giphy.gif" }
                    { Name = "Skater" ; BPM = 40.<reps/min> ; GIF = "https://media.giphy.com/media/2WjpfxAI5MvC9Nl8U7/giphy.gif" }
                    { Name = "Squat foot touch" ; BPM = 22.<reps/min> ; GIF = "https://media.giphy.com/media/2WjpfxAI5MvC9Nl8U7/giphy.gif" }
                    { Name = "Squats sautés" ; BPM = 40.<reps/min> ; GIF = "https://media.giphy.com/media/2WjpfxAI5MvC9Nl8U7/giphy.gif" }
                    { Name = "Step up" ; BPM = 30.<reps/min> ; GIF = "https://media.giphy.com/media/2WjpfxAI5MvC9Nl8U7/giphy.gif" }
                    { Name = "Coude genoux" ; BPM = 30.<reps/min> ; GIF = "https://media.giphy.com/media/2WjpfxAI5MvC9Nl8U7/giphy.gif" }
                |]
            Workouts =
                [|
                    {
                        Id = 101
                        Name = "Poids du corps 1 (new)"
                        Notes = "https://90daylc.thibaultgeoffray.com/mes-routines/phase-1/routine-38"
                        Template = hiit
                        Settings = None
                        Exercises =
                            [|
                                "Squat foot touch"
                                "Montées de genoux"
                                "Pompes en T"
                                "Burpees"
                            |]
                    }
                    {
                        Id = 102
                        Name = "Poids du corps 4 (new)"
                        Notes = "https://90daylc.thibaultgeoffray.com/mes-routines/phase-1/routine-49"
                        Template = { hiit with Cycles = 4 }
                        Settings = None
                        Exercises =
                            [|
                                "Skater"
                                "Bear plank"
                                "Step up"
                                "Break dancer"
                                "Step up"
                            |]
                    }
                    {
                        Id = 103
                        Name = "Poids du corps 2 (old)"
                        Notes = "https://90daylc.thibaultgeoffray.com/mes-routines/phase-1/routine-12"
                        Template = hiit
                        Settings = None
                        Exercises =
                            [|
                                "Squats sautés"
                                "Montées de genoux"
                                "Mountain climbers"
                                "Jumping jack"
                            |]
                    }
                    {
                        Id = 200
                        Name = "Vacuum"
                        Notes = ""
                        Settings =
                            Some [| ("key_workout_sound_time", "12")
                                    ("key_workout_sound_latest_seconds", "value_sound_wood_2")
                                    ("key_workout_sound_last_seconds_work", "value_sound_glass") |]
                        Template =
                            {
                                Warmup = duration 30.<sec>
                                WarmupCycles = None
                                Cycles = 6
                                Work = duration 30.<sec>
                                Rest = duration 25.<sec>
                                Recovery = duration 25.<sec>
                                CoolDown = None
                            }
                        Exercises = [| "Hold" |]
                    }
                    {
                        Id = 300
                        Name = "Méditation"
                        Notes = ""
                        Settings = None
                        Template =
                            {
                                Warmup = duration 10.<sec>
                                WarmupCycles = None
                                Cycles = 1
                                Work = duration (15.<min> |%| 30.<sec>)
                                Rest = duration 10.<sec>
                                Recovery = duration 10.<sec>
                                CoolDown = Some (duration 10.<sec>)
                            }
                        Exercises = [| "~~~~" |]
                    }
                |]
        }

    [<EntryPoint>]
    let main argv =

        config
        |> WorkoutConfigurationConverter.convertConfig
        |> Array.map WorkoutIntervalExpander.create
        |> OutputFileFormat.createResult
        |> OutputSerialization.serialize
        |> writeToFile "result.workout"

        printfn "Done, written to file"
        0 // return an integer exit code
