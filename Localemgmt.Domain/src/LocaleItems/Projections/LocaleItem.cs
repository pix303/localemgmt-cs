using EventSourcingStore;
using Localemgmt.Domain.LocaleItems.Events;


namespace Localemgmt.Domain.LocaleItems.Projections;


public class LocaleItem : AbstractLocaleItem, IAggregate
{
	public bool IsLangReference { get; set; }
	public string Context { get; set; } = null!;
	public List<TranslationItem> Translations { get; set; } = new();


	private void Apply(LocaleItemCreationEvent @event)
	{
		BaseLocalePersistenceEvent e = new(@event.Lang, @event.Content, @event.UserId, @event.AggregateId);
		base.Create(e);
		Context = @event.Context;
		IsLangReference = true;
	}

	private void Apply(LocaleItemUpdateEvent @event)
	{
		BaseLocalePersistenceEvent e = new(@event.Lang, @event.Content, @event.UserId, @event.AggregateId);
		base.Update(e);
		Context = @event.Context;
	}


	private void Apply(TranslationItemCreationEvent @event)
	{
		TranslationItem item = new();
		item.Apply(@event);

		this.Translations.Add(item);
	}

	private void Apply(TranslationItemUpdateEvent @event)
	{
		TranslationItem? item = Translations.Find(item => item.Lang == @event.Lang);
		item?.Apply(@event);
	}


	public void Apply(StoreEvent @event)
	{
		switch (@event)
		{
			case LocaleItemCreationEvent localeItemCreated:
				Apply(localeItemCreated);
				break;

			case LocaleItemUpdateEvent localeItemUpdated:
				Apply(localeItemUpdated);
				break;

			case TranslationItemCreationEvent traslationCreated:
				Apply(traslationCreated);
				break;

			case TranslationItemUpdateEvent traslationUpdated:
				Apply(traslationUpdated);
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
