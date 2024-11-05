using Localemgmt.Domain.LocaleItems.Events;

namespace Localemgmt.Domain.LocaleItems.Projections;


public abstract class AbstractLocaleItem
{
	public Guid Id { get; set; }
	public string AggregateId { get; set; } = null!;
	public string Content { get; set; } = null!;
	public string Lang { get; set; } = null!;
	public DateTime? CreatedAt { get; set; } = null!;
	public string CreatedBy { get; set; } = null!;
	public DateTime? UpdatedAt { get; set; } = null!;
	public string UpdatedBy { get; set; } = null!;


	protected void Create(BaseLocalePersistenceEvent evt)
	{
		Id = Guid.NewGuid();
		AggregateId = evt.AggregateId;
		CreatedAt = DateTime.UtcNow;
		CreatedBy = evt.UserId;

		Content = evt.Content;
		Lang = evt.Lang;
	}

	protected void Update(BaseLocalePersistenceEvent @event)
	{
		UpdatedAt = DateTime.UtcNow;
		UpdatedBy = @event.UserId;

		Content = @event.Content;
		Lang = @event.Lang;
	}

}
