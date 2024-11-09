using Localemgmt.Domain.LocaleItems.Projections;
using Localemgmt.Domain.LocaleItems.Events;


namespace Localemgmt.Domain.Test;

public class LocaleItemsUnitTest
{
	[Fact]
	public void LocaleItemCreation()
	{
		var lastContent = "this is a test and in english";
		var user = "user123";
		var lang = "en";
		var context = "Default";

		LocaleItemCreationEvent creationEvent = new(lang, lastContent, user, context);
		TranslationItemUpdatedEvent updateEvent = new(lang, lastContent + " come on", user, creationEvent.AggregateId);
		TranslationItemUpdatedEvent updateEvent2 = new(lang, lastContent + " come on on on on ", user, creationEvent.AggregateId);
		TranslationItemUpdatedEvent updateEvent3 = new("it", "questo un test in italiano", user, creationEvent.AggregateId);

		IList<BaseLocalePersistenceEvent> events = [
			creationEvent,
			updateEvent,
			updateEvent2,
			updateEvent3,
		];

		LocaleItemAggregate item = new LocaleItemAggregate();
		item.Reduce(events);

		Assert.Equal(item.Context, context);
		Assert.Equal(item.LangReference, lang);
		Assert.True(item.Translations.Count == 2);
		Assert.True(item.Translations[0].Lang == "en");
		Assert.True(item.Translations[1].Lang == "it");
	}
}
