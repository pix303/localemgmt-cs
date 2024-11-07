namespace Localemgmt.Contracts.LocaleItem;

public record LocaleItemMutationResponse(
	string AggregateId
);

public record LocaleItemListItem
{
	public string AggregateId { get; set; } = null!;
	public string Lang { get; set; } = null!;
	public string Context { get; set; } = null!;
	public string Content { get; set; } = null!;
	public string UpdatedAt { get; set; } = null!;
	public string UpdatedBy { get; set; } = null!;

	public LocaleItemListItem(
		string aggregateId,
		string lang,
		string context,
		string content,
		string updatedAt,
		string updatedBy
	)
	{
		this.AggregateId = aggregateId;
		this.Lang = lang;
		this.Context = context;
		this.Content = content;
		this.UpdatedAt = updatedAt;
		this.UpdatedBy = updatedBy;
	}

	// public LocaleItemListItem() { }
};

public record LocaleItemDetail
(
	string AggregateId,
	string Data
);
