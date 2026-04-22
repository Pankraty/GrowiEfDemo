global using GrowiEfDemo;
using System.Text.Json.Serialization;
using FastEndpoints;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;
using Scalar.AspNetCore;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>  
    c.CustomSchemaIds(type => $"{type.DeclaringType?.Name}{type.Name}"));
builder.Services.AddFastEndpoints();
builder.Services.Configure<JsonOptions>(options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddSingleton<ISerializerDataContractResolver>(sp =>
    new JsonSerializerDataContractResolver(sp.GetRequiredService<IOptions<JsonOptions>>().Value.SerializerOptions));

var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("Default"));
var dataSource = dataSourceBuilder.Build();
builder.Services.AddDbContext<DemoDbContext>(opt => opt.UseNpgsql(dataSource, 
    npgsql => npgsql.MapEnum<RateMode>()));

var app = builder.Build();

app.MapSwagger("/openapi/{documentName}.json");
app.MapScalarApiReference();

using (var scope = app.Services.CreateScope())
{
    await scope.ServiceProvider.GetRequiredService<DemoDbContext>()
        .Database.MigrateAsync();
}

app.UseFastEndpoints();
app.Run();