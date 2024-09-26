using EventSourcingStore;
using Localemgmt.Domain.LocaleItems.Events;

namespace Localemgmt.Domain.LocaleItems.Projections;


public class TranslationItem
{
	public Guid Id { get; set; }
	public string Content { get; set; } = null!;
	public string Lang { get; set; } = null!;
	public DateTime? CreatedAt { get; set; } = null!;
	public string CreatedBy { get; set; } = null!;
	public DateTime? UpdatedAt { get; set; } = null!;
	public string UpdatedBy { get; set; } = null!;


	protected void Apply(TranslationItemCreationEvent @event)
	{
		Id = Guid.NewGuid();
		CreatedAt = DateTime.UtcNow;
		CreatedBy = @event.UserId;

		Content = @event.Content;
		Lang = @event.Lang;
	}

	protected void Apply(TranslationItemUpdateEvent @event)
	{
		UpdatedAt = DateTime.UtcNow;
		UpdatedBy = @event.UserId;

		Content = @event.Content;
		Lang = @event.Lang;
	}

	public void Apply(StoreEvent @event)
	{
		switch (@event)
		{
			case TranslationItemCreationEvent translationCreated:
				Apply(translationCreated);
				break;


			case TranslationItemUpdateEvent translationUpdated:
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

