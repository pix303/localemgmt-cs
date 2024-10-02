using ErrorOr;
using FluentValidation;
using Localemgmt.Application.Commons;

namespace Localemgmt.Application.LocaleItem.Commands.Add;

public class AddLocaleItemCommand : ICommand
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

	public ErrorOr<bool> Validate()
	{
		var validator = new AddLocaleItemCommandValidator();
		var validation = validator.Validate(this);
		if (validation.IsValid)
		{
			return true;
		}
		return ErrorOr.Error.Validation(description: validation.Errors.First().ErrorMessage);
	}
}


public class AddLocaleItemCommandValidator : AbstractValidator<AddLocaleItemCommand>
{
	public AddLocaleItemCommandValidator()
	{
		RuleFor(c => c.Lang.Length)
		.InclusiveBetween(2, 6)
		.WithMessage("Lang is required and must be between 2 and 6 length");

		RuleFor(c => c.Content)
		.NotEmpty()
		.WithMessage("Content is required");

		RuleFor(c => c.Context)
		.NotEmpty()
		.WithMessage("Context is required");

		RuleFor(c => c.UserId)
		.NotEmpty()
		.WithMessage("UserId is required");
	}
}
