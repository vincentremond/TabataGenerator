namespace RepsTabataGenerator

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
            Settings: Settings
        }

    let convertConfig (root: Config): WorkoutSimpleDescription array =
        let findByName name =
            root.Exercises
            |> Seq.filter (fun x -> x.Name = name)
            |> Seq.tryExactlyOne

        let convertConfig' (workout:Workout) =
            {
                Id = workout.Id
                Title = workout.Name
                Notes = workout.Notes
                Warmup = workout.Template.Warmup
                WarmupCycles = workout.Template.WarmupCycles
                Cycles = workout.Template.Cycles
                Work = workout.Template.Work
                Rest = workout.Template.Rest
                Recovery = workout.Template.Recovery
                CoolDown = workout.Template.CoolDown
                Settings = workout.Settings
                Exercises =
                    workout.Exercises
                    |> Array.map (fun e ->
                        match (findByName e) with
                        | Some t -> ExerciseReps(t.Name, t.BPM, t.GIF)
                        | None -> ExerciseDuration(e))
            }

        root.Workouts |> Array.map convertConfig'
