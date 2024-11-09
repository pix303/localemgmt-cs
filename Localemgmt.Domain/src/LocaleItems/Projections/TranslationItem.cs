using EventSourcingStore;
using Localemgmt.Domain.LocaleItems.Events;

namespace Localemgmt.Domain.LocaleItems.Projections;


public class TranslationItem
{
	public string Content { get; set; } = null!;
	public string Lang { get; set; } = null!;

	public DateTime? CreatedAt { get; set; } = null!;
	public string CreatedBy { get; set; } = null!;
	public DateTime? UpdatedAt { get; set; } = null!;
	public string UpdatedBy { get; set; } = null!;


	protected void Apply(TranslationItemUpdatedEvent evt)
	{
		UpdatedAt = DateTime.UtcNow;
		UpdatedBy = evt.UserId;

		Content = evt.Content;
		Lang = evt.Lang;
	}

	public void Apply(StoreEvent evt)
	{
		switch (evt)
		{
			case TranslationItemUpdatedEvent translationUpdated:
				Apply(translationUpdated);
				break;
		}
	}

	public void Reduce(IList<StoreEvent> @events)
	{
		foreach (var e in @events)
		{
			Apply(e);
		}
	}

}

