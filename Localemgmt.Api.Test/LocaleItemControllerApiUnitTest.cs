using ErrorOr;
using EventSourcingStore;
using EventSourcingStore.Test;
using Localemgmt.Api.Config;
using Localemgmt.Api.Controllers;
using Localemgmt.Application.LocaleItem.Commands.Add;
using Localemgmt.Contracts.LocaleItem;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Localemgmt.Api.Test;

public class LocaleItemControllerApiUnitTest
{
    string _lang = "it";
    string _context = "Default";
    string _baseContent = "this is a test";
    string _userId = "123";

    ISender _sender;

    public LocaleItemControllerApiUnitTest()
    {
        var services = new ServiceCollection();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddTransient<IRequestHandler<AddLocaleItemCommand, ErrorOr<StoreEvent>>, AddLocaleItemCommandHandler>();
        services.AddScoped<IEventStore, MockEventStore>();

        MapsterConfig.RegisterMapsterConfiguration();
        var serviceProvider = services.BuildServiceProvider();
        _sender = serviceProvider.GetRequiredService<ISender>();

    }

    [Fact]
    public async void AddItem_Ok()
    {
        var controller = new LocaleItemMutationController(_sender);
        LocaleItemMutationRequest request = new(
            Lang: _lang,
            Content: _baseContent,
            Context: _context,
            UserId: _userId
        );

        var result = await controller.Add(request);
        var okResult = Assert.IsAssignableFrom<ObjectResult>(result);
        Assert.NotNull(okResult);
        Assert.Equal(okResult.StatusCode, StatusCodes.Status200OK);
        var resultValue = okResult.Value as LocaleItemMutationResponse;
        Assert.NotNull(resultValue);
        Assert.NotNull(resultValue.AggregateId);
    }

    [Fact]
    public void AddItem_BadRequest()
    {
        var controller = new LocaleItemMutationController(_sender);
        LocaleItemMutationRequest request = new(
            Lang: "",
            Content: _baseContent,
            Context: _context,
            UserId: _userId
        );
        AssertBadRequest(request, "Lang");
    }

    [Fact]
    public void AddItem_BadRequest2()
    {
        var controller = new LocaleItemMutationController(_sender);
        LocaleItemMutationRequest request = new(
            Lang: _lang,
            Content: "",
            Context: _context,
            UserId: _userId
        );
        AssertBadRequest(request, "Content");
    }

    [Fact]
    public void AddItem_BadRequest3()
    {
        var controller = new LocaleItemMutationController(_sender);
        LocaleItemMutationRequest request = new(
            Lang: _lang,
            Content: _baseContent,
            Context: "",
            UserId: ""
        );
        AssertBadRequest(request, "Context");
    }

    [Fact]
    public void AddItem_BadRequest4()
    {
        LocaleItemMutationRequest request = new(
            Lang: "",
            Content: "",
            Context: "",
            UserId: ""
        );
        AssertBadRequest(request, "Lang");
    }


    private async void AssertBadRequest(LocaleItemMutationRequest request, string descriptionPart)
    {
        var controller = new LocaleItemMutationController(_sender);
        var result = await controller.Add(request);

        var problem = Assert.IsType<ObjectResult>(result);
        Assert.Equal(problem.StatusCode, StatusCodes.Status400BadRequest);
        var problemDetails = Assert.IsType<ProblemDetails>(problem.Value);
        Assert.Equal(problemDetails.Status, StatusCodes.Status400BadRequest);
        Assert.Equal("Bad request", problemDetails.Title);
        Assert.NotNull(problemDetails.Detail);
        Assert.NotEmpty(problemDetails.Detail);
        Assert.Contains(descriptionPart, problemDetails.Detail);
    }
}
