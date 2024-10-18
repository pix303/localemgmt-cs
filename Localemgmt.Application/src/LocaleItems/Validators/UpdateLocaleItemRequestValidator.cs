using FluentValidation;
using Localemgmt.Contracts.LocaleItem;

namespace Localemgmt.Application.LocaleItem.Validators;

public class UpdateLocaleItemRequestValidator : AbstractValidator<LocaleItemUpdateRequest>
{
	public UpdateLocaleItemRequestValidator()
	{
		RuleFor(r => r.AggregateId)
		.NotEmpty()
		.Length(default(Guid).ToString().Length)
		.WithMessage("AggregateId is required");

		RuleFor(r => r.UserId)
		.NotEmpty()
		.WithMessage("UserId is required");
	}
}
