using System.ComponentModel.DataAnnotations;

namespace Localemgmt.Contracts.LocaleItem;

public record LocaleItemMutationRequest
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
