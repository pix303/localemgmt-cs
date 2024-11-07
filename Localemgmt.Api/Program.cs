// using Localemgmt.Api.Middleware;
using Localemgmt.Application;
using Localemgmt.Infrastructure;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.IdentityModel.Tokens;
// using System.Text;
using Localemgmt.Api.Middleware;
using MassTransit;
using Localemgmt.Api.Consumer;



var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddLogging();
builder.Services.AddMassTransit(configuration =>
{
    configuration.SetKebabCaseEndpointNameFormatter();
    configuration.AddConsumer<LocaleItemProjectionConsumer, LocaleItemProjectionConsumerDefinition>();
    // configuration.UsingInMemory((context, cfg) =>
    // {
    //     cfg.ConfigureEndpoints(context);
    // });
    configuration.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddControllers();

// builder.Services.AddAuthentication(opts =>
// {
//     opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//     opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//     opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

// }).AddJwtBearer(opts =>
// {
//     var skey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!));
//     opts.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
//         ValidAudience = builder.Configuration["JwtSettings:Audience"],
//         IssuerSigningKey = skey,
//         ValidateIssuerSigningKey = true,
//         ValidateAudience = true,
//         ValidateIssuer = true,
//         ValidateLifetime = true,
//     };

// });

// builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseMiddleware<ErrorHandlerMiddleware>();
// app.UseExceptionHandler("/error");
// app.UseAuthentication();
// app.UseAuthorization();
app.MapControllers();
app.Run();
