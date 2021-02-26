namespace RepsTabataGenerator

open System
open RepsTabataGenerator
open RepsTabataGenerator.Model
open RepsTabataGenerator.LabelFactory
open RepsTabataGenerator.WorkoutConfigurationConverter

module WorkoutIntervalExpander =

    type DetailedInterval =
        | Prepare of Duration
        | WorkDuration of Label * Duration
        | WorkReps of Label * Reps * BPM * GIF
        | Rest of Duration
        | Recovery of Duration
        | CoolDown of Duration

    type DetailedWorkout =
        {
            Id: int
            Title: Label
            Notes: string
            Intervals: DetailedInterval array
            Settings: Settings
        }

    let getRepsCount (bpm: BPM) (duration: Duration) (bpmAdjust: float): Reps =
        bpm * (secondsToMinutes duration) * bpmAdjust
        |> ceiling

    let createRepsInterval label (bpm: BPM) gif duration bpmAdjust =
        DetailedInterval.WorkReps(label, (getRepsCount bpm duration bpmAdjust), (bpm * bpmAdjust), gif)

    let createInterval ex pre exi exc cyi cyc acy d bpmAdjust: DetailedInterval =
        let createLabel' lab = LabelFactory.createLabel pre exi exc cyi cyc acy lab

        match ex with
        | ExerciseDuration (l) -> DetailedInterval.WorkDuration((createLabel' l), d)
        | ExerciseReps (l, b, g) -> createRepsInterval (createLabel' l) b g d bpmAdjust

    let mapi (arr: Exercise array) =
        let mapi' index item = ((index + 1), item)
        arr |> Array.mapi mapi'

    let postExerciseInterval warmup exi exc cyi cyc description =
        let lastCycle = cyi = cyc
        let lastExercise = exi = exc

        match (lastExercise, lastCycle, warmup, description.CoolDown) with
        | (false, _, _, _) -> Some(DetailedInterval.Rest description.Rest)
        | (true, true, true, _) -> Some(DetailedInterval.Recovery description.Recovery)
        | (true, false, _, _) -> Some(DetailedInterval.Recovery description.Recovery)
        | (true, true, false, Some d) -> Some(DetailedInterval.CoolDown d)
        | (true, true, false, None) -> None

    let createIntervals (description: WorkoutSimpleDescription): DetailedInterval array =
        let rec createIntervals' (description: WorkoutSimpleDescription): seq<DetailedInterval> =
            seq {
                yield DetailedInterval.Prepare(description.Warmup)

                let exc = description.Exercises.Length

                match description.WarmupCycles with
                | Some warmup ->
                    let cyc = warmup
                    for cyi = 1 to cyc do
                        for (exi, exercise) in (description.Exercises |> mapi) do
                            yield createInterval exercise "Warmup" exi exc cyi cyc (Some description.Cycles) description.Work 0.75

                            match postExerciseInterval true exi exc cyi cyc description with
                            | Some x -> yield x
                            | None -> ()

                | _ -> ()

                let cyc = description.Cycles
                for cyi = 1 to cyc do
                    for (exi, exercise) in (description.Exercises |> mapi) do
                        yield createInterval exercise "" exi exc cyi cyc None description.Work 1.0

                        match postExerciseInterval false exi exc cyi cyc description with
                        | Some x -> yield x
                        | None -> ()
            }

        description |> createIntervals' |> Seq.toArray

    let private getNotes (description: WorkoutSimpleDescription) =
        let getExLabel ex =
            match ex with
            | ExerciseDuration l -> l
            | ExerciseReps (l, _, _) -> l

        description.Notes
        + "\n\nExercises:\n"
        + (String.Join
            ("\n",
             description.Exercises
             |> Seq.map getExLabel
             |> Seq.map (fun l -> $"• {l}")))

    let create (description: WorkoutSimpleDescription): DetailedWorkout =
        {
            Id = description.Id
            Title = description.Title
            Notes = (getNotes description)
            Intervals = (createIntervals description)
            Settings = description.Settings
        }
