using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TabataGenerator.JsonModels
{
    public static class ModelSerializer
    {
        public class IntervalJsonConverter : JsonConverter
        {
            private readonly JsonSerializerSettings _jsonSerializerSettings;

            public IntervalJsonConverter(MyJsonSerializerSettings myJsonSerializerSettings)
            {
                _jsonSerializerSettings = (myJsonSerializerSettings
                    with
                    {
                        Formatting = Formatting.None,
                    }).ToJsonSerializerSettings();
            }

            public override bool CanConvert(Type objectType) => objectType == typeof(Interval);

            public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
                => writer.WriteRawValue(JsonConvert.SerializeObject(value, _jsonSerializerSettings));

            public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
                => throw new NotImplementedException();
        }

        public static string Serialize(ResultListFile resultListFile)
        {
            var myJsonSerializerSettings = new MyJsonSerializerSettings(
                NullValueHandling: NullValueHandling.Ignore,
                Formatting: Formatting.Indented,
                ContractResolver: new CamelCasePropertyNamesContractResolver()
            );

            var jsonSerializerSettings = myJsonSerializerSettings.ToJsonSerializerSettings();
            jsonSerializerSettings.Converters.Add(new IntervalJsonConverter(myJsonSerializerSettings));

            return JsonConvert.SerializeObject(resultListFile, jsonSerializerSettings);
        }
    }

    public record MyJsonSerializerSettings(
        NullValueHandling NullValueHandling,
        Formatting Formatting,
        IContractResolver ContractResolver
    )
    {
        public JsonSerializerSettings ToJsonSerializerSettings() =>
            new()
            {
                NullValueHandling = NullValueHandling,
                Formatting = Formatting,
                ContractResolver = ContractResolver,
            };
    }
}
