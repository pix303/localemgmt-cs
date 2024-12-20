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
	string UserId,

	string? AggregateId
);


public record LocaleItemUpdateRequest
(
	[Required, MinLength(2), MaxLength(6)]
	string Lang,

	[Required]
	string Content,

	[Required, MinLength(3), MaxLength(64) ]
	string Context,

	[Required]
	string UserId,

	[Required]
	string AggregateId
);

public class LocaleItemSearchRequest
{
	public string? lang { get; set; }
	public string? context { get; set; }
	public string? content { get; set; }

	public LocaleItemSearchRequest
	(
		string? lang,
		string? context,
		string? content
	)
	{
		this.lang = lang;
		this.context = context;
		this.content = content;
	}
};

