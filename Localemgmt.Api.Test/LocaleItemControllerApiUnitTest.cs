using EventSourcingStore;
using Localemgmt.Api.Config;
using Localemgmt.Api.Controllers;
using Localemgmt.Contracts.LocaleItem;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace Localemgmt.Api.Test;


public class StoreFixture
{
    public IEventStore store;
    public ILogger<LocaleItemMutationController> logger;

    public StoreFixture()
    {
        store = new InMemoryEventStore();
        logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<LocaleItemMutationController>();
        MapsterConfig.RegisterMapsterConfiguration();
    }
}


public class LocaleItemControllerApiUnitTest : IClassFixture<StoreFixture>
{
    string _lang = "it";
    string _context = "Default";
    string _baseContent = "this is a test";
    string _userId = "123";
    string _aggregateId = "";

    StoreFixture _fixture;


    public LocaleItemControllerApiUnitTest(StoreFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async void AddItem_Ok()
    {
        await InternalAdd();
    }



    [Fact]
    public async void AddItem_BadRequest()
    {
        LocaleItemCreationRequest request = new(
            Lang: "",
            Content: _baseContent,
            Context: _context,
            UserId: _userId
        );
        var controller = FactoryController();
        var result = await controller.Add(request);
        AssertBadRequest(result, "Lang");
    }

    [Fact]
    public async void AddItem_BadRequest2()
    {
        LocaleItemCreationRequest request = new(
            Lang: _lang,
            Content: "",
            Context: _context,
            UserId: _userId
        );
        var controller = FactoryController();
        var result = await controller.Add(request);
        AssertBadRequest(result, "Content");
    }

    [Fact]
    public async void AddItem_BadRequest3()
    {
        LocaleItemCreationRequest request = new(
            Lang: _lang,
            Content: _baseContent,
            Context: "",
            UserId: _userId
        );
        var controller = FactoryController();
        var result = await controller.Add(request);
        AssertBadRequest(result, "Context");
    }

    [Fact]
    public async void AddItem_BadRequest4()
    {
        var controller = FactoryController();
        LocaleItemCreationRequest request = new(
            Lang: "",
            Content: "",
            Context: "",
            UserId: ""
        );
        var result = await controller.Add(request);
        AssertBadRequest(result, "Lang");
    }


    [Fact]
    public async void UpdateItem_Ok()
    {
        var aggregateId = await InternalAdd();

        LocaleItemUpdateRequest request = new(
            Lang: _lang,
            Content: _baseContent,
            Context: _context,
            UserId: _userId,
            AggregateId: aggregateId
            );

        var controller = FactoryController();
        var result = await controller.Update(request);
        var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);
        Assert.NotNull(okResult);
        Assert.Equal(okResult.StatusCode, StatusCodes.Status200OK);
        var resultValue = okResult.Value as LocaleItemMutationResponse;
        Assert.NotNull(resultValue);
        Assert.NotEmpty(resultValue.AggregateId);
    }


    [Fact]
    public async void UpdateItem_BadRequest()
    {
        var aggregateId = await InternalAdd();

        var controller = FactoryController();
        LocaleItemUpdateRequest request = new(
                    Lang: _lang,
                    Content: _baseContent,
                    Context: _context,
                    UserId: _userId,
                    AggregateId: ""
                    );

        var result = await controller.Update(request);
        AssertBadRequest(result, "not be empty");
    }


    [Fact]
    public async void UpdateItem_BadRequest2()
    {
        var aggregateId = await InternalAdd();

        var controller = FactoryController();
        LocaleItemUpdateRequest request = new(
            Lang: _lang,
            Content: _baseContent,
            Context: _context,
            UserId: _userId,
            AggregateId: "-"
        );

        var result = await controller.Update(request);
        AssertBadRequest(result, "required");
    }


    private async Task<string> InternalAdd()
    {
        var controller = FactoryController();
        LocaleItemCreationRequest request = new(
            Lang: _lang,
            Content: _baseContent,
            Context: _context,
            UserId: _userId
        );

        var result = await controller.Add(request);
        var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);
        Assert.NotNull(okResult);
        Assert.Equal(okResult.StatusCode, StatusCodes.Status200OK);
        var resultValue = okResult.Value as LocaleItemMutationResponse;
        Assert.NotNull(resultValue);
        Assert.NotEmpty(resultValue.AggregateId);
        return resultValue.AggregateId;
    }

    private void AssertBadRequest(IActionResult result, string descriptionPart)
    {
        var problem = Assert.IsType<ObjectResult>(result);
        Assert.Equal(problem.StatusCode, StatusCodes.Status400BadRequest);
        var problemDetails = Assert.IsType<ProblemDetails>(problem.Value);
        Assert.Equal(problemDetails.Status, StatusCodes.Status400BadRequest);
        Assert.Equal("Bad request", problemDetails.Title);
        Assert.NotNull(problemDetails.Detail);
        Assert.NotEmpty(problemDetails.Detail);
        Assert.Contains(descriptionPart, problemDetails.Detail);
    }


    private LocaleItemMutationController FactoryController()
    {
        var controller = new LocaleItemMutationController(_fixture.store, _fixture.logger, null);
        return controller;
    }
}
