using EventSourcingStore;

namespace Localemgmt.Domain.LocaleItems.Events;

public static class LocaleItemEventTypes
{
	public const string AddLocaleItem = "ADD_LOCALE_ITEM";
	public const string UpdateLocaleItem = "UPDATE_LOCALE_ITEM";
	public const string AddTranslationItem = "ADD_TRANSLATION_ITEM";
	public const string UpdateTranslationItem = "UPDATE_TRANSLATION_ITEM";
}


public class BaseLocalePersistenceEvent : StoreEvent
{
	public string Lang;
	public string Content;
	public string UserId;

	public BaseLocalePersistenceEvent(
		string lang,
		string content,
		string user
	)
	{
		Lang = lang;
		Content = content;
		UserId = user;
	}
}


public class TranslationItemCreationEvent : BaseLocalePersistenceEvent
{

	public TranslationItemCreationEvent(
		string lang,
		string content,
		string user
	) : base(
		lang,
		content,
		user
	)
	{
		Type = LocaleItemEventTypes.AddTranslationItem;
	}
};

public class TranslationItemUpdateEvent : BaseLocalePersistenceEvent
{
	public TranslationItemUpdateEvent(
		string lang,
		string content,
		string user
	) : base(
		lang,
		content,
		user
	)
	{
		Type = LocaleItemEventTypes.UpdateTranslationItem;
	}
};

public class TranslationItemDeleteEvent : EventBase
{
};




public abstract class LocaleItemPersistenceEvent : BaseLocalePersistenceEvent
{
	public string Context;
	public LocaleItemPersistenceEvent(
		string lang,
		string content,
		string user,
		string context
	) : base(
		lang,
		content,
		user
	)
	{
		Context = context;
	}
};

public class LocaleItemCreationEvent : LocaleItemPersistenceEvent
{
	public LocaleItemCreationEvent(
		string lang,
		string content,
		string user,
		string context
	) : base(
		lang,
		content,
		user,
		context
	)
	{
		Type = LocaleItemEventTypes.AddLocaleItem;
	}
};

public class LocaleItemUpdateEvent : LocaleItemPersistenceEvent
{
	public LocaleItemUpdateEvent(
		string lang,
		string content,
		string user,
		string context
	) : base(
		lang,
		content,
		user,
		context
	)
	{
		Type = LocaleItemEventTypes.UpdateLocaleItem;
	}
};

public class LocaleItemDeleteEvent : EventBase
{
};


