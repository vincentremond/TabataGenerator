module RepsTabataGenerator.Tests.TestLabel

open FsUnit
open NUnit.Framework
open RepsTabataGenerator.WorkoutIntervalExpander

[<Test>]
let Test1 () =
    (createLabel "Warmup" 1 2 1 2 None "Lorem")
    |> should equal "Warmup\n[Ex. 1/2 • Cycle 1/2]\nLorem"

    (createLabel "Warmup" 1 1 2 2 (Some 3) "Lorem")
    |> should equal "Warmup\n[Cycle 2/2+3]\nLorem"

    (createLabel "" 1 1 1 1 None "Lorem") |> should equal "\n[Cycle 1/1]\nLorem"

    (createLabel "" 1 1 1 1 (Some 1) "Lorem")
    |> should equal "\n[Cycle 1/1+1]\nLorem"
