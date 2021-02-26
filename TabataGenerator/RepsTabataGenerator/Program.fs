namespace RepsTabataGenerator

open System
open Configuration
open RepsTabataGenerator
open RepsTabataGenerator.Model

module EntryPoint =

    let writeToFile path contents =
        System.IO.File.WriteAllText(path, contents)

    type ExDesc = {
        Name: Label
        BPM: BPM
        GIF: GIF
    }        

    [<EntryPoint>]
    let main _ =

        let exercises = [|
            { Name = "Bear plank" ; BPM = 42.<reps/min> ; GIF ="https://media.giphy.com/media/2WjpfxAI5MvC9Nl8U7/giphy.gif" }
            { Name = "Break dancer" ; BPM = 23.<reps/min> ; GIF ="https://media.giphy.com/media/2WjpfxAI5MvC9Nl8U7/giphy.gif" }
            { Name = "Burpees" ; BPM = 17.<reps/min> ; GIF ="https://pas-bien.net/divers/tabata/burpees.gif" }
            { Name = "Jumping jack" ; BPM = 120.<reps/min> ; GIF ="https://media.giphy.com/media/2WjpfxAI5MvC9Nl8U7/giphy.gif" }
            { Name = "Montées de genoux" ; BPM = 135.<reps/min> ; GIF ="https://pas-bien.net/divers/tabata/montees-genoux.gif" }
            { Name = "Mountain climbers" ; BPM = 100.<reps/min> ; GIF ="https://pas-bien.net/divers/tabata/montain-climbers.gif" }
            { Name = "Pompes en T" ; BPM = 12.<reps/min> ; GIF ="https://media.giphy.com/media/2WjpfxAI5MvC9Nl8U7/giphy.gif" }
            { Name = "Skater" ; BPM = 40.<reps/min> ; GIF ="https://media.giphy.com/media/2WjpfxAI5MvC9Nl8U7/giphy.gif" }
            { Name = "Squat foot touch" ; BPM = 22.<reps/min> ; GIF ="https://media.giphy.com/media/2WjpfxAI5MvC9Nl8U7/giphy.gif" }
            { Name = "Squats sautés" ; BPM = 40.<reps/min> ; GIF ="https://media.giphy.com/media/2WjpfxAI5MvC9Nl8U7/giphy.gif" }
            { Name = "Step up" ; BPM = 30.<reps/min> ; GIF ="https://media.giphy.com/media/2WjpfxAI5MvC9Nl8U7/giphy.gif" }
            { Name = "Coude genoux" ; BPM = 30.<reps/min> ; GIF ="https://media.giphy.com/media/2WjpfxAI5MvC9Nl8U7/giphy.gif" }
        |] 

        let asEx input =
            let findEx s =
                match exercises |> Seq.filter (fun e -> e.Name = s) |> Seq.tryExactlyOne with
                | Some e -> Exercise.ExerciseReps (e.Name, e.BPM, e.GIF)
                | None -> Exercise.ExerciseDuration (s)
                        
            input |> Array.map findEx
            
        let hiitMachine =
            {
                Warmup = (7.<min> |%| 30.<sec>)
                WarmupCycles = Some 0
                Cycles = 16
                Work = 30.<sec>
                Rest = 30.<sec>
                Recovery = 30.<sec>
                CoolDown = Some (30.<sec>)
            }

        let hiit =
            {
                Warmup = 20.<sec>
                WarmupCycles = Some 2
                Cycles = 5
                Work = 25.<sec>
                Rest = 35.<sec>
                Recovery = 35.<sec>
                CoolDown = Some (3.<min> |%| 0.<sec>)
            }

        let circuitTraining =
            {
                Warmup = 20.<sec>
                WarmupCycles = Some 2
                Cycles = 5
                Work = 30.<sec>
                Rest = 4.<sec>
                Recovery = (1.<min> |%| 15.<sec>)
                CoolDown = Some (3.<min> |%| 0.<sec>)
            }

        [|
                    {
                        Id = 101
                        Title = "Poids du corps 1 (new)"
                        Notes = "https://90daylc.thibaultgeoffray.com/mes-routines/phase-1/routine-38"
                        Template = hiit
                        Settings = None
                        Exercises =
                            [|
                                "Squat foot touch"
                                "Montées de genoux"
                                "Pompes en T"
                                "Burpees"
                            |] |> asEx
                    }
                    {
                        Id = 102
                        Title = "Poids du corps 4 (new)"
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
                            |] |> asEx
                    }
                    {
                        Id = 103
                        Title = "Poids du corps 2 (old)"
                        Notes = "https://90daylc.thibaultgeoffray.com/mes-routines/phase-1/routine-12"
                        Template = hiit
                        Settings = None
                        Exercises =
                            [|
                                "Squats sautés"
                                "Montées de genoux"
                                "Mountain climbers"
                                "Jumping jack"
                            |] |> asEx
                    }
                    {
                        Id = 200
                        Title = "Vacuum"
                        Notes = ""
                        Settings =
                            Some [| ("key_workout_sound_time", "12")
                                    ("key_workout_sound_latest_seconds", "value_sound_wood_2")
                                    ("key_workout_sound_last_seconds_work", "value_sound_glass") |]
                        Template =
                            {
                                Warmup = 30.<sec>
                                WarmupCycles = None
                                Cycles = 6
                                Work = 30.<sec>
                                Rest = 25.<sec>
                                Recovery = 25.<sec>
                                CoolDown = None
                            }
                        Exercises = [| "Hold" |] |> asEx
                    }
                    {
                        Id = 300
                        Title = "Méditation"
                        Notes = ""
                        Settings = None
                        Template =
                            {
                                Warmup = 10.<sec>
                                WarmupCycles = None
                                Cycles = 1
                                Work = (15.<min> |%| 30.<sec>)
                                Rest = 10.<sec>
                                Recovery = 10.<sec>
                                CoolDown = Some (10.<sec>)
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
