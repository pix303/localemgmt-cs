using System.ComponentModel.DataAnnotations;

namespace Localemgmt.Contracts.LocaleItem;

public record LocaleItemCreationRequest
(
	[Required, MinLength(2), MaxLength(6)]
	string Lang,

	[Required]
	string Content,

	[Required, MinLength(3), MaxLength(64) ]
	string Context,

	[Required]
	string UserId
);


public record LocaleItemUpdateRequest
(
	string? Lang,

	string? Content,

	string? Context,

	[Required]
	string UserId,

	[Required]
	string AggregateId
);
