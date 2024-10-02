using ErrorOr;
using Localemgmt.Domain.LocaleItems.Events;
using EventSourcingStore;
using MediatR;
using Mapster;

namespace Localemgmt.Application.LocaleItem.Commands.Add;


public class AddLocaleItemCommandHandler : IRequestHandler<AddLocaleItemCommand, ErrorOr<StoreEvent>>
{
	private IEventStore _store;

	public AddLocaleItemCommandHandler(IEventStore store)
	{
		_store = store;
	}

	public async Task<ErrorOr<StoreEvent>> Handle(AddLocaleItemCommand request, CancellationToken token)
	{
		var e = request.Adapt<LocaleItemCreationEvent>();
		var result = await _store.Append(e);
		return result;
	}
}
