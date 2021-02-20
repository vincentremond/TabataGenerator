module RepsTabataGenerator.Tests.TestBpm

open System
open NUnit.Framework
open RepsTabataGenerator.WorkoutIntervalExpander

[<Test>]
let Test1 () =
    let Test1' bpm duration bpmAdjust expected =
        let repsCount =
            getRepsCount bpm (TimeSpan.Parse(duration)) bpmAdjust

        Assert.AreEqual(expected, repsCount)

    Test1' 60 "00:01:00" 1.0 60
    Test1' 60 "00:01:00" 0.5 30
    Test1' 4 "00:00:15" 1.0 1
    Test1' 4 "00:00:16" 1.0 2
