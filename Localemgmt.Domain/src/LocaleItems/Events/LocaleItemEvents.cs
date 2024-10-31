using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using EventSourcingStore;

namespace Localemgmt.Domain.LocaleItems.Events
{

	public static class LocaleItemEventTypes
	{
		public const string LocaleItemAdded = "LOCALE_ITEM_ADDED";
		public const string LocaleItemUpdated = "LOCALE_ITEM_UPDATED";
		public const string TranslationItemAdded = "TRANSLATION_ITEM_ADDED";
		public const string TranslationItemUpdated = "TRANSLATION_ITEM_UPDATED";

		public static List<JsonDerivedType> DerivatedTypes = new()
		{
			new JsonDerivedType(typeof(BaseLocalePersistenceEvent),"BaseLocalePersistenceEvent"),
			new JsonDerivedType(typeof(LocaleItemCreationEvent),"LocaleItemCreationEvent"),
			new JsonDerivedType(typeof(LocaleItemUpdateEvent),"LocaleItemUpdateEvent"),
		};
	}


	[JsonPolymorphic]
	[JsonDerivedType(typeof(BaseLocalePersistenceEvent), "BaseLocalePersistenceEvent")]
	[JsonDerivedType(typeof(LocaleItemCreationEvent), "LocaleItemCreationEvent")]
	[JsonDerivedType(typeof(LocaleItemUpdateEvent), "LocaleItemUpdateEvent")]
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
					DateTime createdAt,
					string? aggregateId,
					string eventType,
					string lang,
					string content,
					string userId
					) : base(id, createdAt, aggregateId ?? "", eventType)
		{
			Lang = lang;
			Content = content;
			UserId = userId;
		}
	}




	public abstract class LocaleItemPersistenceEvent : BaseLocalePersistenceEvent
	{
		[JsonPropertyName("context")]
		public string Context { get; set; }

		public LocaleItemPersistenceEvent(
			string eventType,
			string lang,
			string content,
			string userId,
			string context,
			string? aggreateId
		) : base(
			eventType,
			lang,
			content,
			userId,
			aggreateId
		)
		{
			Context = context;
		}


		[JsonConstructor]
		public LocaleItemPersistenceEvent(
					string id,
					DateTime createdAt,
					string aggreateId,
					string eventType,
					string lang,
					string content,
					string userId,
					string context
				) : base(
					id,
					createdAt,
					aggreateId,
					eventType,
					lang,
					content,
					userId
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
			LocaleItemEventTypes.LocaleItemAdded,
			lang,
			content,
			userId,
			context,
			Guid.NewGuid().ToString()
		)
		{
		}

		[JsonConstructor]
		public LocaleItemCreationEvent(
					string id,
					DateTime createdAt,
					string aggregateId,
					string eventType,
					string lang,
					string content,
					string userId,
					string context
				) : base(
					id,
					createdAt,
					aggregateId,
					eventType,
					lang,
					content,
					userId,
					context
				)
		{
		}
	};

	public class LocaleItemUpdateEvent : LocaleItemPersistenceEvent
	{
		public LocaleItemUpdateEvent(
			string eventType,
			string lang,
			string content,
			string userId,
			string context,
			string aggreateId
		) : base(
			LocaleItemEventTypes.LocaleItemUpdated,
			lang,
			content,
			userId,
			context,
			aggreateId
		)
		{
		}

		[JsonConstructor]
		public LocaleItemUpdateEvent(
			string id,
			DateTime createdAt,
			string aggregateId,
			string eventType,
			string lang,
			string content,
			string userId,
			string context
		) : base(
			id,
			createdAt,
			aggregateId,
			eventType,
			lang,
			content,
			userId,
			context
		)
		{
		}

		override public string ToString()
		{
			return $"{base.ToString()} content: {this.Content} context: {this.Context}";
		}

	};










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
			EventType = LocaleItemEventTypes.TranslationItemAdded;
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
			EventType = LocaleItemEventTypes.TranslationItemUpdated;
		}
	};

}

