using Microsoft.EntityFrameworkCore;
using OrderFinanceControl.Data;
using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Data.Repositories.Sql;
using OrderFinanceControl.Middlewares;
using OrderFinanceControl.UseCases;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services
    .AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddScoped<ICustomerRepository, CustomerSqlRepository>();
builder.Services.AddScoped<IProductRepository, ProductSqlRepository>();
builder.Services.AddScoped<IOrderRepository, OrderSqlRepository>();

builder.Services.AddScoped<CreateCustomerUseCase>();
builder.Services.AddScoped<CreateProductUseCase>();
builder.Services.AddScoped<CreateOrderUseCase>();


builder.Services.AddDbContext<OrderFinanceControlDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/api-docs");
}
app.UseExceptionHandler();

app.Run();
