module RepsTabataGenerator.Model

open System
open Microsoft.FSharp.Core.LanguagePrimitives

[<Measure>]
type sec

[<Measure>]
type min

[<Measure>]
type reps

let inline (|%|) (m:float<min>) (s:float<sec>) : float<sec> = (m * 60.<sec> / 1.<min> + s)
type Duration = float<sec>

let duration (s:float<sec>):Duration = s
let secondsToMinutes (s:float<sec>):float<min> = s * 1.<min> / 60.<sec>  
let ceiling<[<Measure>]'u>(x: float<'u>): float<'u> = Math.Ceiling(float x) |> FloatWithMeasure


type Reps = float<reps>
type Label = string
type TemplateId = string
type GIF = string
type BPM = float<reps/min>

type Settings = (string * string) array option