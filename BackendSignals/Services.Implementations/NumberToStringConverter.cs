using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BackendSignals.Services.Implementations
{
    public class NumberToStringConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // If the token is a number, convert it to string
            if (reader.TokenType == JsonTokenType.Number)
            {
                // Use GetDecimal to avoid precision loss, then call ToString
                return reader.GetDecimal().ToString();
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                return reader.GetString();
            }
            throw new JsonException($"Unexpected token parsing string. Expected Number or String, got {reader.TokenType}.");
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }

}
