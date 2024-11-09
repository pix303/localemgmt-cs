using FluentValidation;
using Localemgmt.Contracts.LocaleItem;

namespace Localemgmt.Application.LocaleItem.Validators;

public class UpdateLocaleItemRequestValidator : AbstractValidator<LocaleItemUpdateRequest>
{
	public UpdateLocaleItemRequestValidator()
	{
		RuleFor(r => r.AggregateId)
		.NotEmpty()
		.WithMessage("AggregateId is required eh");

		RuleFor(r => r.UserId)
		.NotEmpty()
		.WithMessage("UserId is required");
	}
}
