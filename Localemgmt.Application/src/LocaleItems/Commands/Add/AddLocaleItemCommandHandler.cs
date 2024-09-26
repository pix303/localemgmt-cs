using ErrorOr;
using Localemgmt.Domain.LocaleItems.Events;
using EventSourcingStore;
using MediatR;
using Mapster;

namespace Localemgmt.Application.LocaleItem.Commands.Add;


public class AddLocaleItemCommandHandler : IRequestHandler<AddLocaleItemCommand, ErrorOr<bool>>
{
	private IEventStore _store;

	public AddLocaleItemCommandHandler(IEventStore store)
	{
		_store = store;
	}

	public async Task<ErrorOr<bool>> Handle(AddLocaleItemCommand request, CancellationToken token)
	{
		var e = request.Adapt<LocaleItemCreationEvent>();
		Console.WriteLine(e);
		var result = await _store.Append(e);
		return true;
	}
}
