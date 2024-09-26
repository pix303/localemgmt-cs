using Localemgmt.Application.LocaleItem.Commands.Add;

namespace Localemgmt.Application.LocaleItem.Commands.Update
{
	public class UpdateLocaleItemCommand : AddLocaleItemCommand
	{

		public string AggregateId { get; set; }

		public UpdateLocaleItemCommand(
					string lang,
					string content,
					string userId,
					string context,
					string aggregateId
		) : base(
					lang,
					content,
					userId,
					context
		)
		{
			AggregateId = aggregateId;
		}
	};
}

