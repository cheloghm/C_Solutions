using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Titanic.Api.Repositories;
using Titanic.Api.Settings;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Titanic.Api.Api.Repositories;
using System;
using Microsoft.Extensions.Hosting;
using System.Linq;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region MongoDbSettings
var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();

BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
//BsonSerializer.RegisterSerializer(new DateTimeSerializer(BsonType.String));

builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    //var settings =  builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
    //return new MongoClient(settings.ConnectionString);
    return new MongoClient(mongoDbSettings.ConnectionString);
});

builder.Services.AddSingleton<IPassengersRepository, MongoDbPassengersRepository>();

//builder.Services.AddControllers();
builder.Services.AddControllers(options =>
{
options.SuppressAsyncSuffixInActionNames = false;
});

// if (!builder.Environment.IsDevelopment())
// {
//     builder.Services.AddHttpsRedirection(options =>
//     {
//         options.RedirectStatusCode = (int)HttpStatusCode.PermanentRedirect;
//         options.HttpsPort = 443;
//     });
// }

#endregion

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks()
    .AddMongoDb(mongoDbSettings.ConnectionString, 
    name: "mongodb", 
    timeout: TimeSpan.FromSeconds(3),
    tags: new[]{"ready"});

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready", new HealthCheckOptions{
    Predicate = (check) => check.Tags.Contains("ready"),
    ResponseWriter = async(context, report) =>
    {
        var result = JsonSerializer.Serialize(
            new{
                status = report.Status.ToString(),
                checks = report.Entries.Select(entry => new {
                    name = entry.Key,
                    status = entry.Value.Status.ToString(),
                    exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "None",
                    duration = entry.Value.Duration.ToString()
                })
            }
        );

        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(result);
    }
});
app.MapHealthChecks("/health/live", new HealthCheckOptions{
    Predicate = (_) => false
});

app.Run();
