using EventSourcingStore;

namespace Localemgmt.Domain.LocaleItems.Events
{

	public static class LocaleItemEventTypes
	{
		public const string LocaleItemAdded = "LOCALE_ITEM_ADDED";
		public const string LocaleItemUpdated = "LOCALE_ITEM_UPDATED";
		public const string TranslationItemAdded = "TRANSLATION_ITEM_ADDED";
		public const string TranslationItemUpdated = "TRANSLATION_ITEM_UPDATED";
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
			Type = LocaleItemEventTypes.TranslationItemAdded;
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
			Type = LocaleItemEventTypes.TranslationItemUpdated;
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
			Type = LocaleItemEventTypes.LocaleItemAdded;
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
			Type = LocaleItemEventTypes.LocaleItemUpdated;
		}
	};

	public class LocaleItemDeleteEvent : StoreEvent
	{
	};
}

