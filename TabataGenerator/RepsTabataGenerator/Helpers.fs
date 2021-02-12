module RepsTabataGenerator.Helpers

let inline (|?) a b =
    match a with
    | Some r -> r
    | None -> b

let inline (|??) a b =
    match a with
    | Some _ -> a
    | None -> b
