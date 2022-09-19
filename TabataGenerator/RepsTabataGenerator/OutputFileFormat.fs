namespace RepsTabataGenerator

open System
open System.Collections.Generic
open System.Security.Cryptography
open Newtonsoft.Json
open RepsTabataGenerator.Model
open RepsTabataGenerator.WorkoutIntervalExpander
open RepsTabataGenerator.Tools


module OutputFileFormat =

    type Interval =
        {
            addSet: bool
            bpm: int
            cycle: int
            cyclesCount: int
            isRepsMode: bool
            reps: int
            tabata: int
            tabatasCount: int
            time: int
            [<JsonProperty("type")>]
            intervalType: int
            [<JsonProperty(NullValueHandling = NullValueHandling.Ignore)>]
            description: string
            [<JsonProperty(NullValueHandling = NullValueHandling.Ignore)>]
            url: string
        }

    type Workout =
        {
            colorId: int
            coolDown: int
            cycles: int
            doNotRepeatFirstPrepareAndLastCoolDown: bool
            id: int
            intervals: Interval array
            intervalsSetsCount: int
            isFavorite: bool
            isRestRepsMode: bool
            isWorkRepsMode: bool
            [<JsonProperty(NullValueHandling = NullValueHandling.Ignore)>]
            notes: string
            prepare: int
            rest: int
            restBetweenTabatas: int
            restBetweenWorkoutsReps: int
            restBetweenWorkoutsRepsMode: bool
            restBetweenWorkoutsTime: int
            restBpm: int
            [<JsonProperty(NullValueHandling = NullValueHandling.Ignore)>]
            restDescription: string
            restReps: int
            [<JsonProperty(NullValueHandling = NullValueHandling.Ignore)>]
            settings: IDictionary<string, string>
            skipLastRestInterval: bool
            skipPrepareAndCoolDownBetweenWorkouts: bool
            tabatasCount: int
            title: string
            work: int
            workBpm: int
            workReps: int
        }

    type ResultListFile =
        {
            settings: obj
            statistics: obj
            workouts: Workout array
            fileVersion: int
            packageName: string
            platform: int
            [<JsonProperty("type")>]
            fileType: int
            versionCode: int
            versionName: string
        }

    let asInt<[<Measure>] 'u> (x: float<'u>) : int = (int (ceiling (float x)))

    let private intervalDuration duration intervalType (description: Label option) =
        {
            addSet = false
            bpm = 0
            cycle = -1
            cyclesCount = -1
            isRepsMode = false
            reps = 0
            tabata = -1
            tabatasCount = -1
            time = duration |> asInt
            intervalType = intervalType
            description = description |> Option.toNull
            url = null
        }

    let private intervalReps intervalType (bpm: BPM) (reps: Reps) (description: Label option) =
        {
            addSet = false
            bpm = bpm |> asInt
            cycle = -1
            cyclesCount = -1
            isRepsMode = true
            reps = reps |> asInt
            tabata = -1
            tabatasCount = -1
            time = 10
            intervalType = intervalType
            description = description |> Option.toNull
            url = null
        }

    let private getIntervals (interval: DetailedInterval) : Interval =
        match interval with
        | DetailedInterval.Prepare d -> intervalDuration d 0 None
        | DetailedInterval.WorkDuration (label, duration) -> intervalDuration duration 1 (Some label)
        | DetailedInterval.WorkReps (label, reps, bpm) -> intervalReps 1 bpm reps (Some label)
        | DetailedInterval.Rest d -> intervalDuration d 2 None
        | DetailedInterval.RestReps (reps, bpm) -> intervalReps 2 bpm reps None
        | DetailedInterval.Recovery d -> intervalDuration d 3 None
        | DetailedInterval.RecoveryReps (reps, bpm) -> intervalReps 3 bpm reps None
        | DetailedInterval.CoolDown d -> intervalDuration d 4 None

    let private getWorkout (workout: DetailedWorkout) : Workout =
        {
            colorId = 2
            coolDown = 0
            cycles = 1
            doNotRepeatFirstPrepareAndLastCoolDown = false
            id = workout.Id
            intervals = workout.Intervals |> Array.map getIntervals
            intervalsSetsCount = 1
            isFavorite = false
            isRestRepsMode = false
            isWorkRepsMode = false
            notes = workout.Notes
            prepare = 10
            rest = 10
            restBetweenTabatas = 0
            restBetweenWorkoutsReps = 0
            restBetweenWorkoutsRepsMode = false
            restBetweenWorkoutsTime = 0
            restBpm = 0
            restDescription = null
            restReps = 0
            settings =
                match workout.Settings with
                | Some arr -> (arr |> dict)
                | None -> null
            skipLastRestInterval = true
            skipPrepareAndCoolDownBetweenWorkouts = false
            tabatasCount = 1
            title = workout.Title
            work = 10
            workBpm = 0
            workReps = 0
        }

    type IntervalJsonConverter() =
        inherit JsonConverter()
        override _.CanConvert(t) = t = typeof<Interval>
        override _.ReadJson(_, _, _, _) = raise (NotImplementedException())

        override _.WriteJson(writer, value, _) =
            let settings =
                JsonSerializerSettings(Formatting = Formatting.None)

            let rawValue =
                JsonConvert.SerializeObject(value, settings)

            writer.WriteRawValue rawValue

    let createResult (workouts: DetailedWorkout array) =
        {
            settings = obj
            statistics = obj
            workouts = workouts |> Array.map getWorkout
            fileVersion = 1
            packageName = "com.evgeniysharafan.tabatatimer"
            platform = 1
            fileType = 3
            versionCode = 502002
            versionName = "5.2.2"
        }

    let serialize obj =
        let settings =
            JsonSerializerSettings(Formatting = Formatting.Indented)

        settings.Converters.Add(new IntervalJsonConverter())
        JsonConvert.SerializeObject(obj, settings)
