module RepsTabataGenerator.LabelFactory

open RepsTabataGenerator.Model

let createLabel (pre: string) (exi: int) (exc: int) (cyi: int) (cyc: int) (acy: int option) (lab: string): string =
    let cycle i c a =
        match a with
        | Some a -> $"Cycle {i}/{c}+{a}"
        | None -> $"Cycle {i}/{c}"

    let exercise i c = $"Ex. {i}/{c}"

    let middlePart =
        match (exc, cyc) with
        | (1, _) -> (cycle cyi cyc acy)
        | (_, 1) -> (exercise exi exc)
        | (_, _) -> (exercise exi exc) + " • " + (cycle cyi cyc acy)

    $"{pre}\n[{middlePart}]\n{lab}"
