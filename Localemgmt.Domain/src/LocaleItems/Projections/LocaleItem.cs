using Localemgmt.Domain.LocaleItems.Events;


namespace Localemgmt.Domain.LocaleItems.Projections;


public class LocaleItem : AbstractLocaleItem, IAggregate<BaseLocalePersistenceEvent>
{
	public bool IsLangReference { get; set; }
	public string Context { get; set; } = null!;
	public List<TranslationItem> Translations { get; set; } = new();


	private void Apply(LocaleItemCreationEvent evt)
	{
		BaseLocalePersistenceEvent e = new(evt.Lang, evt.Content, evt.UserId, evt.AggregateId);
		base.Create(e);
		Context = evt.Context;
		IsLangReference = true;
	}

	private void Apply(LocaleItemUpdateEvent evt)
	{
		BaseLocalePersistenceEvent e = new(evt.Lang, evt.Content, evt.UserId, evt.AggregateId);
		base.Update(e);
		Context = evt.Context;
	}


	private void Apply(TranslationItemCreationEvent evt)
	{
		TranslationItem item = new();
		item.Apply(evt);

		this.Translations.Add(item);
	}

	private void Apply(TranslationItemUpdateEvent evt)
	{
		TranslationItem? item = Translations.Find(item => item.Lang == evt.Lang);
		item?.Apply(evt);
	}


	public void Apply(BaseLocalePersistenceEvent evt)
	{
		switch (evt)
		{
			case LocaleItemCreationEvent localeitemCreated:
				Console.WriteLine("====== created");
				Apply(localeitemCreated);
				break;

			case LocaleItemUpdateEvent localeitemUpdated:
				Console.WriteLine("====== updated");
				Apply(localeitemUpdated);
				break;

			case TranslationItemCreationEvent traslationCreated:
				Apply(traslationCreated);
				break;

			case TranslationItemUpdateEvent traslationUpdated:
				Apply(traslationUpdated);
				break;

			case BaseLocalePersistenceEvent basevt:
				Console.WriteLine("-----errorerere ererer---");
				Console.WriteLine(basevt);
				break;
		}
	}


	public void Reduce(IList<BaseLocalePersistenceEvent> evts)
	{
		foreach (var e in evts)
		{
			Apply(e);
		}
	}
}
