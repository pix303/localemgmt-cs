namespace EventSourcingStore.Test;

public class EventStoreUnitTest
{
	[Fact]
	public async Task DynamoTest()
	{
		var store = new DynamoDBEventStore("localemgmt-store");
		StoreEvent e = new();
		var aId = e.AggregateId;

		// append
		var result = await store.Append(e);
		Assert.False(result.IsError);

		Thread.Sleep(1000);
		StoreEvent e2 = new(aId);
		var result2 = await store.Append(e2);
		Assert.False(result2.IsError);

		Thread.Sleep(1000);
		StoreEvent e3 = new(aId);
		var result3 = await store.Append(e3);
		Assert.False(result3.IsError);

		Thread.Sleep(1000);
		StoreEvent e4 = new(aId);
		var result4 = await store.Append(e4);
		Assert.False(result4.IsError);

		// retrive all
		var resultItems = await store.RetriveByAggregate<StoreEvent>(aId);
		Assert.False(resultItems.IsError);
		var items = resultItems.Value;
		Assert.True(items.Count == 4);
		Assert.Equal(aId, items[0].AggregateId);
		Assert.Equal(aId, items[1].AggregateId);
		Assert.Equal(aId, items[2].AggregateId);
		Assert.Equal(aId, items[3].AggregateId);

		// retrive
		var resultItem = await store.Retrive<StoreEvent>(aId, e.CreatedAt);
		Assert.False(resultItem.IsError);
		var item = resultItem.Value;
		Assert.NotNull(item);
		Assert.Equal(item.AggregateId, aId);
		Assert.NotEmpty(item.Id);
	}

}
