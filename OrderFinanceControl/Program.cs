using Microsoft.EntityFrameworkCore;
using OrderFinanceControl.Data;
using OrderFinanceControl.Data.Configurations;
using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Data.Repositories.Mongo;
using OrderFinanceControl.Middlewares;
using OrderFinanceControl.Settings;
using OrderFinanceControl.UseCases;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
MongoMappings.Register();
builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection("MongoSettings"));

builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services
    .AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddScoped<ICustomerRepository, CustomerMongoRepository>();
builder.Services.AddScoped<IProductRepository, ProductMongoRepository>();
builder.Services.AddScoped<IOrderRepository, OrderMongoRepository>();


builder.Services.AddScoped<CreateCustomerUseCase>();
builder.Services.AddScoped<CreateProductUseCase>();
builder.Services.AddScoped<CreateOrderUseCase>();
builder.Services.AddScoped<MarkOrderAsPaidUseCase>();


builder.Services.AddSingleton<MongoContext>();


var app = builder.Build();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/api-docs");
}
app.UseExceptionHandler();

app.Run();
