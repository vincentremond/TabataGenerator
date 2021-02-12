﻿namespace RepsTabataGenerator

open RepsTabataGenerator.Helpers
open RepsTabataGenerator.Configuration
open RepsTabataGenerator.Model

module WorkoutConfigurationConverter =

    type Exercise =
        | ExerciseDuration of Label
        | ExerciseReps of Label * BPM * GIF

    type WorkoutSimpleDescription =
        {
            Id: int
            Title: Label
            Notes: string
            Warmup: Duration
            WarmupCycles: Option<int>
            Cycles: int
            Work: Duration
            Rest: Duration
            Recovery: Duration
            CoolDown: Duration
            Exercises: Exercise array
        }

    let convertConfig (root: Config): WorkoutSimpleDescription array =
        let findByName name =
            root.Exercises
            |> Seq.filter (fun x -> x.Name = name)
            |> Seq.tryExactlyOne

        let convertConfig' workout =
            let template =
                root.Templates
                |> Seq.filter (fun x -> x.Name = workout.Template)
                |> Seq.exactlyOne

            {
                Id = workout.Id
                Title = workout.Name
                Notes = workout.Notes
                Warmup = template.Warmup
                WarmupCycles =  workout.WarmupCycles |?? template.WarmupCycles
                Cycles = workout.Cycles |? template.Cycles
                Work = template.Work
                Rest = template.Rest
                Recovery = template.Recovery
                CoolDown = template.CoolDown
                Exercises =
                    workout.Exercises
                    |> Array.map (fun e ->
                        match (findByName e) with
                        | Some t -> ExerciseReps(t.Name, t.BPM, t.GIF)
                        | None -> ExerciseDuration(e))
            }

        root.Workouts |> Array.map convertConfig'