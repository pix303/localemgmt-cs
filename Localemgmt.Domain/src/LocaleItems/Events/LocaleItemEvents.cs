using EventSourcingStore;

namespace Localemgmt.Domain.LocaleItems.Events
{

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
			string userId,
			string? aggreateId
		) : base()
		{
			Lang = lang;
			Content = content;
			UserId = userId;
			if (aggreateId is not null)
			{
				AggregateId = aggreateId;
			}
		}
	}


	public class TranslationItemCreationEvent : BaseLocalePersistenceEvent
	{

		public TranslationItemCreationEvent(
			string lang,
			string content,
			string userId
		) : base(
			lang,
			content,
			userId,
			null
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
			string userId
		) : base(
			lang,
			content,
			userId,
			null
		)
		{
			Type = LocaleItemEventTypes.UpdateTranslationItem;
		}
	};

	public class TranslationItemDeleteEvent : StoreEvent
	{
	};




	public abstract class LocaleItemPersistenceEvent : BaseLocalePersistenceEvent
	{
		public string Context;

		public LocaleItemPersistenceEvent(
			string lang,
			string content,
			string userId,
			string context,
			string? aggreateId
		) : base(
			lang,
			content,
			userId,
			aggreateId
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
			string userId,
			string context
		) : base(
			lang,
			content,
			userId,
			context,
			Guid.NewGuid().ToString()
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
			string userId,
			string context,
			string aggreateId
		) : base(
			lang,
			content,
			userId,
			context,
			aggreateId
		)
		{
			Type = LocaleItemEventTypes.UpdateLocaleItem;
		}
	};

	public class LocaleItemDeleteEvent : StoreEvent
	{
	};
}

