using OrderFinanceControl.Data;
using OrderFinanceControl.Data.Configurations;
using OrderFinanceControl.Extensions;
using OrderFinanceControl.Middlewares;
using OrderFinanceControl.Settings;
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

builder.Services
    .AddRepositories()
    .AddUseCases();


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

public partial class Program;
