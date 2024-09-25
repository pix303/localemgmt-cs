namespace EventSourcingStore.Test;

public class EventStoreUnitTest
{
    [Fact]
    public async Task DynamoTest()
    {
        var store = new DynamoDBEventStore("localemgmt-store");
        TestEvent e = new();

        // append
        var result = await store.Append(e);
        Assert.True(result);

        Thread.Sleep(1000);
        await store.Append(e);
        Thread.Sleep(1000);
        await store.Append(e);
        Thread.Sleep(1000);
        await store.Append(e);

        // retrive all
        var items = await store.RetriveByAggregate(e.AggregateId);
        Assert.True(items.Count == 4);
        foreach (var singleItem in items)
        {
            Assert.Equal(singleItem.AggregateId, e.AggregateId);
        }

        // retrive
        var item = await store.Retrive(e.AggregateId, items[0].CreatedAt);
        Assert.NotNull(item);
        Assert.Equal(item.AggregateId, e.AggregateId);
        Assert.NotEmpty(item.Id);
    }

    public class TestEvent : DynamoDBStoreEvent
    {
        public TestEvent()
        {
            AggregateId = Guid.NewGuid().ToString();
        }
    }
}
