using MongoDB.Driver;
using Titanic.Repositories;
using Titanic.Api.Repositories;
using Titanic.Settings;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region MongoDbSettings
//var mongoDbSettings = Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();

builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    var settings =  builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
    return new MongoClient(settings.ConnectionString);
    //return new MongoClient(MongoDbSettings.ConnectionString);
});

builder.Services.AddSingleton<IPassengersRepository, MongoDbPassengersRepository>();

//builder.Services.AddControllers();
builder.Services.AddControllers(options =>
{
options.SuppressAsyncSuffixInActionNames = false;
});

#endregion

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddHealthChecks();
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

app.Run();
