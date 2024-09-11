using Localemgmt.Domain.LocaleItems.Events;

namespace Localemgmt.Domain.LocaleItems.Projections;


public abstract class AbstractLocaleItem
{
	public Guid Id { get; set; }
	public string Content { get; set; } = null!;
	public string Lang { get; set; } = null!;
	public DateTime? CreatedAt { get; set; } = null!;
	public string CreatedBy { get; set; } = null!;
	public DateTime? UpdatedAt { get; set; } = null!;
	public string UpdatedBy { get; set; } = null!;


	protected void Create(LocalePersistenceEvent @event)
	{
		Id = Guid.NewGuid();
		CreatedAt = DateTime.UtcNow;
		CreatedBy = @event.UserId;

		Content = @event.Content;
		Lang = @event.Lang;
	}

	protected void Update(LocalePersistenceEvent @event)
	{
		UpdatedAt = DateTime.UtcNow;
		UpdatedBy = @event.UserId;

		Content = @event.Content;
		Lang = @event.Lang;
	}

}
