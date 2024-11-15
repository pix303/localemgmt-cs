using ErrorOr;
using Localemgmt.Contracts.LocaleItem;
using Localemgmt.Domain.LocaleItems.Projections;

namespace Localemgmt.Infrastructure.Services;

public interface IRetriveService
{
	Task<ErrorOr<List<LocaleItemListItem>>> GetContext(string context, string? lang);
	Task<ErrorOr<List<LocaleItemListItem>>> Search(LocaleItemSearchRequest request);
	Task<ErrorOr<LocaleItemAggregate>> GetDetail(string aggregateId);
	Task<ErrorOr<LocaleItemAggregate>> Match(LocaleItemSearchRequest request);

}
