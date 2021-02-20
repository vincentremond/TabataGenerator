namespace RepsTabataGenerator

module EntryPoint =
    
    let writeToFile path contents =
        System.IO.File.WriteAllText(path, contents)

    [<EntryPoint>]
    let main argv =
        "Config.json"
        |> Configuration.readConfig
        |> WorkoutConfigurationConverter.convertConfig
        |> Array.map WorkoutIntervalExpander.create 
        |> OutputFileFormat.createResult
        |> OutputSerialization.serialize
        |> writeToFile "result.workout"
        printfn "Done, written to file"
        0 // return an integer exit code
