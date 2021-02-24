module RepsTabataGenerator.Tests.TestBpm

open NUnit.Framework
open RepsTabataGenerator.Model
open RepsTabataGenerator.WorkoutIntervalExpander

[<Test>]
let Test1 () =
    let Test1' bpm duration bpmAdjust expected =
        let repsCount =
            getRepsCount bpm duration bpmAdjust

        Assert.AreEqual(expected, repsCount)

    Test1' 60.<reps/min> 60.<sec> 1.0 60
    Test1' 60.<reps/min> 60.<sec> 0.5 30
    Test1' 4.<reps/min> 15.<sec> 1.0 1
    Test1' 4.<reps/min> 16.<sec> 1.0 2
