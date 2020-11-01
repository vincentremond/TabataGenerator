using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TabataGenerator.OutputFormat;

namespace TabataGenerator
{
    internal class MyConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Interval);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.None,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };
            var rawValue = JsonConvert.SerializeObject(value, Formatting.None, jsonSerializerSettings);
            writer.WriteRawValue(rawValue);
        }
    }
}
