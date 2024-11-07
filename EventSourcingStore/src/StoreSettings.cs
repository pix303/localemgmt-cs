namespace EventSourcingStore;

public class StoreSettings
{
	public string DBType { get; set; } = null!;
	public string TableName { get; set; } = null!;
	public string? Connection { get; set; }
}
