using ErrorOr;
using MediatR;

namespace Localemgmt.Application.LocaleItem.Commands.Add;

public class AddLocaleItemCommand : IRequest<ErrorOr<bool>>
{
	public string Lang { get; init; } = null!;
	public string Content { get; init; } = null!;
	public string Context { get; init; } = null!;
	public string UserId { get; init; } = null!;

	public AddLocaleItemCommand(
			string lang,
			string content,
			string userId,
			string context
	)
	{
		Lang = lang;
		Content = content;
		UserId = userId;
		Context = context;
	}
}
