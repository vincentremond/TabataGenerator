namespace RepsTabataGenerator

open System.IO
open Newtonsoft.Json
open Newtonsoft.Json.Converters
open RepsTabataGenerator.Model

module Configuration =
    type Exercise = { Name: Label; BPM: BPM; GIF: GIF }

    type Template =
        {
            Name: TemplateId
            Warmup: Duration
            WarmupCycles: Option<int>
            Cycles: int
            Work: Duration
            Rest: Duration
            Recovery: Duration
            CoolDown: Duration
        }

    type Workout =
        {
            Id: int
            Name: Label
            Notes: string
            Template: TemplateId
            WarmupCycles: Option<int>
            Cycles: Option<int>
            Exercises: string array
        }

    type Config =
        {
            Exercises: Exercise array
            Templates: Template array
            Workouts: Workout array
        }

    let readConfig filename =
        let contents = filename |> File.ReadAllText

        let result =
            JsonConvert.DeserializeObject<Config>(contents, IdiomaticDuConverter())

        result
