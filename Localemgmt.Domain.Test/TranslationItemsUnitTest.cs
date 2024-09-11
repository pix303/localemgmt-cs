using Localemgmt.Domain.LocaleItems.Projections;
using Localemgmt.Domain.LocaleItems.Events;


namespace Localemgmt.Domain.Test;

public class TranslationItemsUnitTest
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

		IList<EventBase> events = [
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
		LocaleItemUpdateEvent updateEvent = new(lang, "this is an edited test", user, "Default");
		LocaleItemUpdateEvent updateEvent2 = new(lang, lastContent, user, "Default");

		TranslationItemCreationEvent tiCreationEvent = new("it", "questo è un test", user);
		TranslationItemUpdateEvent tiUpdateEvent = new("it", "quest è un test modificato", user);
		TranslationItemUpdateEvent tiUpdateEvent2 = new("it", lastContent, user);

		IList<EventBase> events = [
			creationEvent,
			updateEvent,
			updateEvent2,
			tiCreationEvent,
			tiUpdateEvent,
			tiUpdateEvent2
		];

		LocaleItem item = new LocaleItem();
		item.Reduce(events);

		Assert.Equal(item.Content, lastContent);
		Assert.Equal(item.UpdatedBy, user);
		Assert.NotNull(item.Id);
		Assert.True(item.UpdatedAt > item.CreatedAt);
		Assert.True(item.UpdatedBy == item.CreatedBy);
	}
}
