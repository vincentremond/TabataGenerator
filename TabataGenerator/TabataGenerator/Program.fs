
[<Measure>] type bpm
[<Measure>] type min
[<Measure>] type sec

let duration (min:int<min>) (sec:int<sec>) = min * 60<sec> / 1<min> + sec 

type ExerciseName = string
type Rhythm = int<bpm>

type RhythmedExercise = ExerciseName * Rhythm

[<EntryPoint>]
let main argv =
    let ``bear plank`` = RhythmedExercise ("Bear plank", 42<bpm>)
    let ``break dancer`` = RhythmedExercise ("Break dancer", 23<bpm>)
    let ``burpees`` = RhythmedExercise ("Burpees", 13<bpm>)
    let ``jumping jacks`` = RhythmedExercise ("Jumping jack", 125<bpm>)
    let ``knee raises`` = RhythmedExercise ("Knee raises", 140<bpm>)
    let ``mountainClimbers`` = RhythmedExercise("Mountain climbers", 100<bpm>)
    let ``T pumps`` = RhythmedExercise("T-pumps", 13<bpm>)
    let ``skater`` = RhythmedExercise("Skater", 40<bpm>)
    let ``squat foot touch`` = RhythmedExercise("Squat foot touch", 23<bpm>)
    let ``jump squats`` = RhythmedExercise("Jump squats", 40<bpm>)
    let ``step up`` = RhythmedExercise("Step up", 30<bpm>)
    let ``elbow knees`` = RhythmedExercise("Elbow knees", 30<bpm>)
    
    let ``meditation`` = 
        
    printfn "done"
    0
    
        