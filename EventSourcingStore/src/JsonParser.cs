using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using ErrorOr;

namespace EventSourcingStore;

public static class JsonParser
{
	public static ErrorOr<T> Deserialize<T>(string source, IList<JsonDerivedType> derivedTypes)
	{
		try
		{
			var opt = new JsonSerializerOptions
			{
				// TypeInfoResolver = new PolymorphicTypeResolver(typeof(T), derivedTypes),
				IncludeFields = true,
				PropertyNameCaseInsensitive = false,
				AllowOutOfOrderMetadataProperties = true
			};

			Console.WriteLine("-------------------------------------");
			Console.WriteLine(source);
			Console.WriteLine("-------------------------------------");

			var evt = JsonSerializer.Deserialize<T>(source, opt);

			if (evt is null)
			{
				return Error.Failure(code: "Event.Json.Failure.deseralization", description: "event is null ");
			}
			return evt;
		}
		catch (Exception jex)
		{
			return Error.Failure(code: "Event.Json.Failure.deseralization", description: jex.Message);
		}

	}

	public static ErrorOr<string> Serialize<T>(T evt)
	{
		try
		{
			var source = JsonSerializer.Serialize<T>(evt);

			if (source is null)
			{
				return Error.Failure(code: "Event.Json.Failure.seralization", description: "event is null ");
			}
			return source;
		}
		catch (Exception jex)
		{
			return Error.Failure(code: "Event.Json.Failure.seralization", description: jex.Message);
		}

	}

}



public class PolymorphicTypeResolver : DefaultJsonTypeInfoResolver
{
	private readonly Type _baseType = typeof(StoreEvent);
	private readonly IList<JsonDerivedType> _customDerivedTypes;

	public PolymorphicTypeResolver(Type baseType, IList<JsonDerivedType> types)
	{
		_baseType = baseType;
		_customDerivedTypes = types;
	}

	public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
	{
		JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

		Type basePointType = typeof(StoreEvent);
		if (jsonTypeInfo.Type == basePointType)
		{
			jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
			{
				TypeDiscriminatorPropertyName = "$type",
				IgnoreUnrecognizedTypeDiscriminators = true,
				UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
			};
			jsonTypeInfo.PolymorphismOptions.DerivedTypes.Concat(_customDerivedTypes);
		}
		return jsonTypeInfo;
	}
}
