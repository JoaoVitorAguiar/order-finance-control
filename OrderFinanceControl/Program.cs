using Microsoft.EntityFrameworkCore;
using OrderFinanceControl.Data;
using OrderFinanceControl.Extensions;
using OrderFinanceControl.Middlewares;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services
    .AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services
    .AddRepositories()
    .AddUseCases();


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
