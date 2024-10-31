using Localemgmt.Domain.LocaleItems.Projections;
using Localemgmt.Domain.LocaleItems.Events;
using EventSourcingStore;


namespace Localemgmt.Domain.Test;

public class LocaleItemsUnitTest
{
	[Fact]
	public void TranslationItemsProjections_should_update()
	{
		var lastContent = "this is a test and it's edited 2 times";
		var user = "user123";
		var lang = "en";
		TranslationItemCreationEvent creationEvent = new(lang, "this is a test", user);
		TranslationItemUpdateEvent updateEvent = new(lang, "this is an edited test", user);
		TranslationItemUpdateEvent updateEvent2 = new(lang, lastContent, user);

		IList<StoreEvent> events = [
			creationEvent,
			updateEvent,
			updateEvent2
		];


		TranslationItem item = new TranslationItem();
		item.Reduce(events);


		Assert.Equal(item.Content, lastContent);
		Assert.Equal(item.UpdatedBy, user);
		Assert.NotNull(item.Id);
		Assert.True(item.UpdatedAt > item.CreatedAt);
		Assert.True(item.UpdatedBy == item.CreatedBy);
	}

	[Fact]
	public void LocaleItemCreation()
	{
		var lastContent = "this is a test and it's edited 2 times";
		var user = "user123";
		var lang = "en";

		LocaleItemCreationEvent creationEvent = new(lang, "this is a test", user, "Default");
		LocaleItemUpdateEvent updateEvent = new(lang, "this is an edited test", user, "Default", creationEvent.AggregateId);
		LocaleItemUpdateEvent updateEvent2 = new(lang, lastContent, user, "Default", creationEvent.AggregateId);

		TranslationItemCreationEvent tiCreationEvent = new("it", "questo è un test", user);
		TranslationItemUpdateEvent tiUpdateEvent = new("it", "quest è un test modificato", user);
		TranslationItemUpdateEvent tiUpdateEvent2 = new("it", lastContent, user);

		TranslationItemCreationEvent tiCreationEventB = new("fr", "c'est un test", user);
		TranslationItemUpdateEvent tiUpdateEventB = new("fr", "c'est un test modifié", user);
		TranslationItemUpdateEvent tiUpdateEvent2B = new("fr", lastContent, user);

		IList<BaseLocalePersistenceEvent> events = [
			creationEvent,
			updateEvent,
			updateEvent2,
			tiCreationEvent,
			tiUpdateEvent,
			tiUpdateEvent2,
			tiCreationEventB,
			tiUpdateEventB,
			tiUpdateEvent2B
		];

		LocaleItem item = new LocaleItem();
		item.Reduce(events);

		Assert.Equal(item.Content, lastContent);
		Assert.Equal(item.UpdatedBy, user);
		Assert.NotNull(item.Id);
		Assert.True(item.UpdatedAt > item.CreatedAt);
		Assert.True(item.UpdatedBy == item.CreatedBy);
		Assert.True(item.Translations.Count == 2);
		Assert.True(item.Translations[0].Lang == "it");
		Assert.True(item.Translations[1].Lang == "fr");
	}
}
