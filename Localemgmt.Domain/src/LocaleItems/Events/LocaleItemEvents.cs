using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using EventSourcingStore;

namespace Localemgmt.Domain.LocaleItems.Events
{

	public static class LocaleItemEventTypes
	{
		public const string LocaleItemAdded = "LOCALE_ITEM_ADDED";
		public const string TranslationItemUpdated = "TRANSLATION_ITEM_UPDATED";
	}


	[JsonPolymorphic]
	[JsonDerivedType(typeof(BaseLocalePersistenceEvent), "BaseLocalePersistenceEvent")]
	[JsonDerivedType(typeof(LocaleItemCreationEvent), "LocaleItemCreationEvent")]
	[JsonDerivedType(typeof(TranslationItemUpdatedEvent), "TranslationItemUpdatedEvent")]
	public class BaseLocalePersistenceEvent : StoreEvent
	{
		[JsonPropertyName("$type")]
		public string TypeDiscriminator => GetType().Name;

		[JsonPropertyName("lang")]
		public string Lang { get; set; }
		[JsonPropertyName("content")]
		public string Content { get; set; }
		[JsonPropertyName("userId")]
		public string UserId { get; set; }

		public BaseLocalePersistenceEvent() : base()
		{
			Lang = "no-lang";
			Content = "no-content";
			UserId = "no-user";
		}

		public BaseLocalePersistenceEvent(string eventType) : base(eventType)
		{
			Lang = "no-lang";
			Content = "no-content";
			UserId = "no-user";
		}

		public BaseLocalePersistenceEvent(
			string eventType,
			string lang,
			string content,
			string userId
		) : base(eventType)
		{
			Lang = lang;
			Content = content;
			UserId = userId;
		}

		public BaseLocalePersistenceEvent(
			string eventType,
			string lang,
			string content,
			string userId,
			string aggregateId
		) : base(eventType, aggregateId)
		{
			Lang = lang;
			Content = content;
			UserId = userId;
		}

		[JsonConstructor]
		public BaseLocalePersistenceEvent(
			string id,
			string eventType,
			string lang,
			string content,
			string userId,
			string aggregateId,
			DateTime createdAt
		) : base(id, createdAt, aggregateId, eventType)
		{
			Lang = lang;
			Content = content;
			UserId = userId;
		}
	}



	public class LocaleItemCreationEvent : BaseLocalePersistenceEvent
	{
		[JsonPropertyName("context")]
		public string Context { get; set; }

		public LocaleItemCreationEvent(
			string lang,
			string content,
			string userId,
			string context
		) : base(
			LocaleItemEventTypes.LocaleItemAdded,
			lang,
			content,
			userId
		)
		{
			Context = context;
		}

		[JsonConstructor]
		public LocaleItemCreationEvent(
			string id,
			string eventType,
			string lang,
			string content,
			string userId,
			string aggregateId,
			DateTime createdAt,
			string context
		) : base(
			id,
			eventType,
			lang,
			content,
			userId,
			aggregateId,
			createdAt
		)
		{
			Context = context;
		}
	};


	public class TranslationItemUpdatedEvent : BaseLocalePersistenceEvent
	{
		public TranslationItemUpdatedEvent(
			string lang,
			string content,
			string userId,
			string aggregateId
		) : base(
			LocaleItemEventTypes.TranslationItemUpdated,
			lang,
			content,
			userId,
			aggregateId
		)
		{
		}

		[JsonConstructor]
		public TranslationItemUpdatedEvent(
			string id,
			string eventType,
			string lang,
			string content,
			string userId,
			string aggregateId,
			DateTime createdAt
		) : base(
			id,
			eventType,
			lang,
			content,
			userId,
			aggregateId,
			createdAt
		)
		{
		}

		override public string ToString()
		{
			return $"{this.EventType} - {this.Lang} = {this.Content}";
		}
	};

}
