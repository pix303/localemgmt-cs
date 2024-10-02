using ErrorOr;
using EventSourcingStore;
using Localemgmt.Domain.LocaleItems.Events;
using MediatR;
using Mapster;

namespace Localemgmt.Application.LocaleItem.Commands.Update;


public class UpdateLocaleItemCommandHandler : IRequestHandler<UpdateLocaleItemCommand, ErrorOr<StoreEvent>>
{
	private IEventStore _store;

	public UpdateLocaleItemCommandHandler(IEventStore store)
	{
		_store = store;
	}

	public async Task<ErrorOr<StoreEvent>> Handle(UpdateLocaleItemCommand request, CancellationToken token)
	{
		var e = request.Adapt<LocaleItemUpdateEvent>();
		var result = await _store.Append(e);
		return result;
	}
}
