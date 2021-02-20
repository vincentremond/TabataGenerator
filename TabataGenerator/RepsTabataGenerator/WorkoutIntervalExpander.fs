namespace RepsTabataGenerator

open System
open RepsTabataGenerator.Model
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
        }

    let getRepsCount bpm (duration: Duration) bpmAdjust: Reps =
        Convert.ToInt32(Math.Ceiling(bpmAdjust * (float bpm) * duration.TotalSeconds / 60.))

    let createRepsInterval label (bpm: BPM) gif duration bpmAdjust =
        DetailedInterval.WorkReps(label, (getRepsCount bpm duration bpmAdjust), bpm, gif)

    let private createLabel pre exi exc cyi cyc lab =
        $"{pre}\n[Ex. {exi}/{exc} • Cycle {cyi}/{cyc}]\n{lab}"

    let createInterval ex pre exi exc cyi cyc d bpmAdjust: DetailedInterval =
        let createLabel' lab = createLabel pre exi exc cyi cyc lab

        match ex with
        | ExerciseDuration (l) -> DetailedInterval.WorkDuration((createLabel' l), d)
        | ExerciseReps (l, b, g) -> createRepsInterval (createLabel' l) b g d bpmAdjust

    let mapi (arr: Exercise array) =
        let mapi' index item = ((index + 1), item)
        arr |> Array.mapi mapi'

    let postExerciseInterval warmup exi exc cyi cyc description =
        let lastCycle = cyi = cyc
        let lastExercise = exi = exc

        match (lastExercise, lastCycle, warmup) with
        | (false, _, _) -> DetailedInterval.Rest description.Rest
        | (true, true, true) -> DetailedInterval.Recovery description.Recovery
        | (true, false, _) -> DetailedInterval.Recovery description.Recovery
        | (true, true, false) -> DetailedInterval.CoolDown description.CoolDown

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
                            yield createInterval exercise "Warmup" exi exc cyi cyc description.Work 0.75
                            yield postExerciseInterval true exi exc cyi cyc description

                | _ -> ()

                let cyc = description.Cycles
                for cyi = 1 to cyc do
                    for (exi, exercise) in (description.Exercises |> mapi) do
                        yield createInterval exercise "" exi exc cyi cyc description.Work 1.0
                        yield postExerciseInterval false exi exc cyi cyc description
            }

        description |> createIntervals' |> Seq.toArray

    let create (description: WorkoutSimpleDescription): DetailedWorkout =
        {
            Id = description.Id
            Title = description.Title
            Notes = description.Notes
            Intervals = (createIntervals description)
        }
