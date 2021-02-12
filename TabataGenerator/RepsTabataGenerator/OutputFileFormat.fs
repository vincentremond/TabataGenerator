namespace RepsTabataGenerator

open System
open System.Collections.Generic
open System.Text.Json.Serialization
open RepsTabataGenerator.Model
open RepsTabataGenerator.WorkoutIntervalExpander

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
            [<JsonPropertyName("type")>]
            intervalType: int
            [<JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)>]
            description: string
            [<JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)>]
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
            [<JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)>]
            notes: string
            prepare: int
            rest: int
            restBetweenTabatas: int
            restBetweenWorkoutsReps: int
            restBetweenWorkoutsRepsMode: bool
            restBetweenWorkoutsTime: int
            restBpm: int
            [<JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)>]
            restDescription: string
            restReps: int
            [<JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)>]
            settings: Dictionary<string, string>
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
            [<JsonPropertyName("type")>]
            fileType: int
            versionCode: int
            versionName: string
        }

    let seconds (duration: Duration): int =
        let d = duration.TotalSeconds
        Convert.ToInt32(Math.Ceiling(d))

    let private intervalDuration duration intervalType (description: Option<Label>) =
        {
            addSet = false
            bpm = 0
            cycle = -1
            cyclesCount = -1
            isRepsMode = false
            reps = 0
            tabata = -1
            tabatasCount = -1
            time = seconds duration
            intervalType = intervalType
            description =
                match description with
                | Some l -> l
                | None -> null
            url = null
        }

    let private intervalReps intervalType (bpm:BPM) (reps:Reps) description url =
        {
            addSet = false
            bpm = bpm
            cycle = -1
            cyclesCount = -1
            isRepsMode = true
            reps = reps
            tabata = -1
            tabatasCount = -1
            time = 10
            intervalType = intervalType
            description = description
            url = url
        }

    let private getIntervals (interval: DetailedInterval): Interval =
        let t =
            match interval with
            | DetailedInterval.Prepare _ -> 0
            | DetailedInterval.WorkDuration _ -> 1
            | DetailedInterval.WorkReps _ -> 1
            | DetailedInterval.Rest _ -> 2
            | DetailedInterval.Recovery _ -> 3
            | DetailedInterval.CoolDown _ -> 4

        match interval with
        | DetailedInterval.Prepare d -> intervalDuration d t None
        | DetailedInterval.WorkDuration (label, duration) -> intervalDuration duration t (Some label)
        | DetailedInterval.WorkReps (label, reps, bpm, gif) -> intervalReps t bpm reps label gif
        | DetailedInterval.Rest d -> intervalDuration d t None
        | DetailedInterval.Recovery d -> intervalDuration d t None
        | DetailedInterval.CoolDown d -> intervalDuration d t None

    let private getWorkout (workout: DetailedWorkout): Workout =
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
            settings = null
            skipLastRestInterval = true
            skipPrepareAndCoolDownBetweenWorkouts = false
            tabatasCount = 1
            title = workout.Title
            work = 10
            workBpm = 0
            workReps = 0
        }

    let createResult (workouts: DetailedWorkout array) =
        {
            settings = obj
            statistics = obj
            workouts = workouts |> Array.map getWorkout
            fileVersion = 1
            packageName = "com.evgeniysharafan.tabatatimer"
            platform = 1
            fileType = 1
            versionCode = 502002
            versionName = "5.2.2"
        }
