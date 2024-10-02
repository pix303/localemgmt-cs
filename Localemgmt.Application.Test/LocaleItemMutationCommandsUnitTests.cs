using EventSourcingStore;
using EventSourcingStore.Test;
using Localemgmt.Application.LocaleItem.Commands.Add;
using Localemgmt.Api.Config;
using Localemgmt.Application.LocaleItem.Commands.Update;

namespace Localemgmt.Application.Test;


public class StoreFixture
{
    public IEventStore store;
    public StoreFixture()
    {
        store = new MockEventStore();
        MapsterConfig.RegisterMapsterConfiguration();
    }
}

public class LocaleItemMutationCommandsUnitTests : IClassFixture<StoreFixture>
{
    string _lang = "it";
    string _context = "Default";
    string _baseContent = "this is a test";
    string _userId = "123";
    StoreFixture _fixture;

    public LocaleItemMutationCommandsUnitTests(StoreFixture fixture)
    {
        _fixture = fixture;
    }


    [Fact]
    public async void AddCommands_OkRequest()
    {
        var handler = new AddLocaleItemCommandHandler(_fixture.store);

        var command = new AddLocaleItemCommand(
            _lang,
            _context,
            _baseContent,
            _userId
        );

        var result = await handler.Handle(command, CancellationToken.None);
        Assert.NotEmpty(result.Value.AggregateId);
        Assert.Equal(result.Value.Type, Localemgmt.Domain.LocaleItems.Events.LocaleItemEventTypes.AddLocaleItem);
    }

    [Fact]
    public async void UpdateCommands_OkRequest()
    {
        var handler = new UpdateLocaleItemCommandHandler(_fixture.store);
        var id = Guid.NewGuid().ToString();
        var command = new UpdateLocaleItemCommand(
            _lang,
            _context,
            _baseContent,
            _userId,
            id
        );

        var result = await handler.Handle(command, CancellationToken.None);
        Assert.Equal(result.Value.AggregateId, id);
        Assert.Equal(result.Value.Type, Localemgmt.Domain.LocaleItems.Events.LocaleItemEventTypes.UpdateLocaleItem);
    }



}
