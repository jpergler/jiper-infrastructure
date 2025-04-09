using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jiper.Infrastructure.Ids;

public class IdJsonConverter<TId> : JsonConverter<TId> where TId : SemanticTypeId
{
    public override TId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return (TId?)Activator.CreateInstance(typeToConvert, value) ?? throw new InvalidOperationException("Could not create instance of SemanticTypeId");
    }

    public override void Write(Utf8JsonWriter writer, TId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}