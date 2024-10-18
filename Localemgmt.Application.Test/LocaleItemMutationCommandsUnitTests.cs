using EventSourcingStore;
using Localemgmt.Api.Config;

namespace Localemgmt.Application.Test;


public class StoreFixture
{
    public IEventStore store;
    public StoreFixture()
    {
        store = new InMemoryEventStore();
        MapsterConfig.RegisterMapsterConfiguration();
    }
}

public class LocaleItemApplicationUnitTests : IClassFixture<StoreFixture>
{
    string _lang = "it";
    string _context = "Default";
    string _baseContent = "this is a test";
    string _userId = "123";
    StoreFixture _fixture;

    public LocaleItemApplicationUnitTests(StoreFixture fixture)
    {
        _fixture = fixture;
    }
}
