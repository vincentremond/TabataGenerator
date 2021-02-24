module RepsTabataGenerator.Model

open System

[<Measure>]
type sec

[<Measure>]
type min

let inline (|%|) (m:int<min>) (s:int<sec>) : int<sec> = (m * 60<sec> / 1<min> + s)
type Duration = TimeSpan

let duration (s:int<sec>):Duration = TimeSpan.FromSeconds((float) s)

type Reps = int
type Label = string
type TemplateId = string
type GIF = string
type BPM = int

type Settings = (string * string) array option