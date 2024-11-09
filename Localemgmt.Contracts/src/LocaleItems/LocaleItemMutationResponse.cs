namespace Localemgmt.Contracts.LocaleItem;

public record LocaleItemMutationResponse(
	string AggregateId
);


public record LocaleItemDetail
(
	string AggregateId,
	string Data
);
