module RepsTabataGenerator.Tests.TestBpm

open FsUnit
open NUnit.Framework
open RepsTabataGenerator.Model
open RepsTabataGenerator.WorkoutIntervalExpander

[<Test>]
let Test1 () =
    getRepsCount 60.<reps/min> 60.<sec> 1.0
    |> should equal 60

    getRepsCount 60.<reps/min> 60.<sec> 0.5
    |> should equal 30

    getRepsCount 4.<reps/min> 15.<sec> 1.0
    |> should equal 1

    getRepsCount 4.<reps/min> 16.<sec> 1.0
    |> should equal 2
