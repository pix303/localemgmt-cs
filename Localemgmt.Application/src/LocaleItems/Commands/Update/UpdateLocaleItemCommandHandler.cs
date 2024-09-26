using ErrorOr;
using EventSourcingStore;
using Localemgmt.Domain.LocaleItems.Events;
using MediatR;
using Mapster;

namespace Localemgmt.Application.LocaleItem.Commands.Update;


public class UpdateLocaleItemCommandHandler : IRequestHandler<UpdateLocaleItemCommand, ErrorOr<bool>>
{
	private IEventStore _store;

	public UpdateLocaleItemCommandHandler(IEventStore store)
	{
		_store = store;
	}

	public async Task<ErrorOr<bool>> Handle(UpdateLocaleItemCommand request, CancellationToken token)
	{
		var e = request.Adapt<LocaleItemCreationEvent>();
		Console.WriteLine(e);
		var result = await _store.Append(e);
		return result;
	}
}
