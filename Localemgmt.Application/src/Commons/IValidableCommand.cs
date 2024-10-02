using ErrorOr;
using EventSourcingStore;
using MediatR;

namespace Localemgmt.Application.Commons;


public interface IValidable
{
	ErrorOr<bool> Validate();
}

public interface ICommand : IRequest<ErrorOr<StoreEvent>>, IValidable;
