using BuildingBlock.Behaviors;
using Catalog.API.Product.CreateProduct;
using Catalog.API.Product.GetProductByCategory;
using Catalog.API.Product.GetProductById;
using Catalog.API.Product.GetProducts;
using Catalog.API.Product.UpdateProduct;
using Catalog.API.Products.DeleteProduct;
using FluentValidation;
using Marten;
using System.Reflection;
using BuildingBlock.Exceptions.Handler;
using Catalog.API.Data;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;

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
}).UseLightweightSessions();
if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);
var app = builder.Build();
app.UseHealthChecks("/health",new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter=UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapCarter();
app.Run();