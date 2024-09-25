namespace EventSourcingStore
{
    public interface IDynamoDBEventStore
    {
        Task Append<T>(T @event) where T : StoreEvent;
        Task<List<StoreEvent>> Retrive(string aggregateId);
    }
}
