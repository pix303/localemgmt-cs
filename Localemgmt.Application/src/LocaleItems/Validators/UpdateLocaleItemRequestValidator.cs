using FluentValidation;

namespace Localemgmt.Application.LocaleItem.Validators;

public class UpdateLocaleItemRequestValidator : AddLocaleItemRequestValidator
{
	public UpdateLocaleItemRequestValidator() : base()
	{
		RuleFor(r => r.AggregateId)
		.NotEmpty()
		.WithMessage("AggregateId is required");
	}
}
