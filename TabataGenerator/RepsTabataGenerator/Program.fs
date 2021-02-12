namespace RepsTabataGenerator

open System.Text.Json

module EntryPoint =
    
    let writeToFile path contents =
        System.IO.File.WriteAllText(path, contents)
        
    let serialize obj =
        JsonSerializer.Serialize (obj, JsonSerializerOptions( WriteIndented = true )) 

    [<EntryPoint>]
    let main argv =
        "Config.json"
        |> Configuration.readConfig
        |> WorkoutConfigurationConverter.convertConfig
        |> Array.map WorkoutIntervalExpander.create 
        |> OutputFileFormat.createResult
        |> serialize
        |> writeToFile "result.workout"

        0 // return an integer exit code
