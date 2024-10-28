using System.Text.Json;
using ErrorOr;

namespace EventSourcingStore;

public static class JsonParser
{
	public static ErrorOr<T> Deserialize<T>(string source)
	{
		try
		{
			var opt = new JsonSerializerOptions
			{
				IncludeFields = true,
				PropertyNameCaseInsensitive = false,
			};
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
}
