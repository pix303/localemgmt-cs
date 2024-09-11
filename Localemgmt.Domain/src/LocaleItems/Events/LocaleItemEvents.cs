namespace Localemgmt.Domain.LocaleItems.Events;

public class LocalePersistenceEvent : EventBase
{
	public string Lang;
	public string Content;
	public string UserId;

	public LocalePersistenceEvent(
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


public class TranslationItemCreationEvent : LocalePersistenceEvent
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
	{ }
};

public class TranslationItemUpdateEvent : LocalePersistenceEvent
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
	{ }
};

public class TranslationItemDeleteEvent : EventBase
{
};




public abstract class LocaleItemPersistenceEvent : LocalePersistenceEvent
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
	{ }
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
	{ }
};

public class LocaleItemDeleteEvent : EventBase
{
};


