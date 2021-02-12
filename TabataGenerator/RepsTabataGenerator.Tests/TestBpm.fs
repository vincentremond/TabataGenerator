module RepsTabataGenerator.Tests.TestBpm

open System
open NUnit.Framework
open RepsTabataGenerator.WorkoutIntervalExpander

[<Test>]
let Test1 () =
    let Test1' bpm duration expected =
        let repsCount =
            getRepsCount bpm (TimeSpan.Parse(duration))

        Assert.AreEqual(expected, repsCount)

    Test1' 60 "00:01:00" 60
    Test1' 4 "00:00:15" 1
    Test1' 4 "00:00:16" 2
