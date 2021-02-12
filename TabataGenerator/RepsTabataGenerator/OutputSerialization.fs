namespace RepsTabataGenerator

open System
open Newtonsoft.Json
open RepsTabataGenerator.OutputFileFormat

type IntervalJsonConverter() =
    inherit JsonConverter()
//    new() = IntervalJsonConverter()
    override _.CanConvert(t) = t = typeof<Interval>
    override _.ReadJson(_, _, _, _) = raise (NotImplementedException())
    override _.WriteJson(writer, value, _) =
        let settings = JsonSerializerSettings(Formatting = Formatting.None)
        let rawValue = JsonConvert.SerializeObject(value, settings)
        writer.WriteRawValue rawValue

module OutputSerialization =

    let serialize obj =
        let settings =
            JsonSerializerSettings(Formatting = Formatting.Indented)

        settings.Converters.Add(new IntervalJsonConverter())
        JsonConvert.SerializeObject(obj, settings)
