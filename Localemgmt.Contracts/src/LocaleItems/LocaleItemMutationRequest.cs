using System.ComponentModel.DataAnnotations;

namespace Localemgmt.Contracts.LocaleItem;

public record LocaleItemMutationRequest
(

	[MinLength(2), MaxLength(6)]
	string Lang,

	string Content,

	[MinLength(3), MaxLength(64) ]
	string Context,

	string UserId
);
