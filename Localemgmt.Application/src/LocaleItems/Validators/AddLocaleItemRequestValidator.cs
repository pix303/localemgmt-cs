using FluentValidation;
using Localemgmt.Contracts.LocaleItem;

namespace Localemgmt.Application.LocaleItem.Validators;

public class AddLocaleItemRequestValidator : AbstractValidator<LocaleItemMutationRequest>
{
	public AddLocaleItemRequestValidator()
	{
		RuleFor(r => r.Lang.Length)
		.InclusiveBetween(2, 6)
		.WithMessage("Lang is required and must be between 2 and 6 length");

		RuleFor(r => r.Content)
		.NotEmpty()
		.WithMessage("Content is required");

		RuleFor(r => r.Context)
		.NotEmpty()
		.WithMessage("Context is required");

		RuleFor(r => r.UserId)
		.NotEmpty()
		.WithMessage("UserId is required");
	}
}
