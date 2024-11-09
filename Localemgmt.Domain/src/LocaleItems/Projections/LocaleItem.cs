using Localemgmt.Domain.LocaleItems.Events;


namespace Localemgmt.Domain.LocaleItems.Projections;


public class LocaleItemAggregate : IAggregate<BaseLocalePersistenceEvent>
{
	public string AggregateId { get; set; } = null!;
	public string LangReference { get; set; } = null!;
	public string Context { get; set; } = null!;
	public List<TranslationItem> Translations { get; set; } = new();

	public TranslationItem? GetTranslationByLang(string lang)
	{
		return this.Translations.SingleOrDefault(item => item.Lang == lang);
	}


	private void Apply(LocaleItemCreationEvent evt)
	{
		TranslationItem t = new();
		TranslationItemUpdatedEvent te = new(evt.Lang, evt.Content, evt.UserId, evt.AggregateId);
		t.Apply(te);

		Translations.Add(t);
		Context = evt.Context;
		LangReference = evt.Lang;
		AggregateId = evt.AggregateId;
	}


	private void Apply(TranslationItemUpdatedEvent evt)
	{
		TranslationItem? t = GetTranslationByLang(evt.Lang);
		t?.Apply(evt);
	}


	public void Apply(BaseLocalePersistenceEvent evt)
	{
		switch (evt)
		{
			case LocaleItemCreationEvent localeitemCreated:
				Apply(localeitemCreated);
				break;

			case TranslationItemUpdatedEvent traslationUpdated:
				Apply(traslationUpdated);
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


	public override string ToString()
	{
		var t = this.GetTranslationByLang(this.LangReference);
		return $"aggId: {this.AggregateId} context: {this.Context} - lang: {t?.Lang} - content: {t?.Content}";
	}
}



public record LocaleItemListItem
{
	public string AggregateId { get; set; } = null!;
	public string Lang { get; set; } = null!;
	public string Context { get; set; } = null!;
	public string Content { get; set; } = null!;
	public string UpdatedAt { get; set; } = null!;
	public string UpdatedBy { get; set; } = null!;

	public LocaleItemListItem(
		string aggregateId,
		string lang,
		string context,
		string content,
		string updatedAt,
		string updatedBy
	)
	{
		this.AggregateId = aggregateId;
		this.Lang = lang;
		this.Context = context;
		this.Content = content;
		this.UpdatedAt = updatedAt;
		this.UpdatedBy = updatedBy;
	}

	public LocaleItemListItem(LocaleItemAggregate localeItem, string lang)
	{
		this.AggregateId = localeItem.AggregateId;
		this.Context = localeItem.Context;
		var t = localeItem.GetTranslationByLang(lang);
		this.Content = t?.Content ?? $"Error lang {lang} not find";
		this.Lang = lang;
		this.UpdatedAt = (t?.UpdatedAt is not null ? t.UpdatedAt.ToString() : t?.CreatedAt.ToString()) ?? DateTime.Now.ToString();
		this.UpdatedBy = (t?.UpdatedBy is not null ? t.UpdatedBy : t?.CreatedBy) ?? "no-user";
	}
};
