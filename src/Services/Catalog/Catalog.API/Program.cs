using Catalog.API.Products.CreateProduct;
using Catalog.API.Products.GetProductById;
using Catalog.API.Products.GetProducts;
using Marten;
using System.Reflection;
var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
});
//builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter();
builder.Services.AddCarter(configurator: c =>
{
   
  c.WithModule<CreateProductEndpoint>();
  c.WithModule<GetProductsEndPoint>();
  c.WithModule<GetProductByIdEndPoint>();

});

builder.Services.AddMarten(opts=>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();
var app = builder.Build();
app.MapCarter();
app.Run();
