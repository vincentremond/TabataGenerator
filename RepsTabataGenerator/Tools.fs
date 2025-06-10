namespace RepsTabataGenerator.Tools

module Option =
    let toNull (x: 'a option) : 'a =
        match x with
        | Some value -> value
        | None -> null
