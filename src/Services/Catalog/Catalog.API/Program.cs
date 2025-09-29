using BuildingBlock.Behaviors;
using BuildingBlock.Exceptions.Handler;
using Catalog.API.Data;
using Catalog.API.Product.CreateProduct;
using Catalog.API.Product.GetProductByCategory;
using Catalog.API.Product.GetProductById;
using Catalog.API.Product.GetProducts;
using Catalog.API.Product.UpdateProduct;
using Catalog.API.Products.DeleteProduct;
using FluentValidation;
using HealthChecks.UI.Client;
using Marten;
using Microsoft.OpenApi.Models;
using Weasel.Core;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Catalog API",
        Version = "v1",
        Description = "Catalog service API for eShop microservices",
        Contact = new OpenApiContact
        {
            Name = "Development Team"
        }
    });
});

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddCarter();
builder.Services.AddCarter(configurator: c =>
{
    c.WithModule<CreateProductEndpoint>();
    c.WithModule<GetProductsEndPoint>();
    c.WithModule<GetProductByIdEndPoint>();
    c.WithModule<GetProductByCategoryEndPoint>();
    c.WithModule<UpdateProductEndPoint>();
    c.WithModule<DeleteProductEndPoint>();
});

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    opts.AutoCreateSchemaObjects = AutoCreate.All;
}).UseLightweightSessions().
ApplyAllDatabaseChangesOnStartup();
if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);
var app = builder.Build();

// Configure Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API V1");
    });
}

app.UseHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapCarter();
app.Run();