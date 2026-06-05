using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using OrderFinanceControl.Data;
using OrderFinanceControl.Settings;
using Testcontainers.MongoDb;
using Xunit;

namespace OrderFinanceControl.Tests.Integration;

public sealed class TestMongoFixture : IAsyncLifetime
{
    private readonly MongoDbContainer _container = new MongoDbBuilder().Build();

    public WebApplicationFactory<Program> Factory { get; private set; } = default!;
    public string DatabaseName => $"OrderFinanceControlTests_{Guid.NewGuid():N}";
    public string ConnectionString => _container.GetConnectionString();

    public Task InitializeAsync()
    {
        return InitializeFactoryAsync();
    }

    private async Task InitializeFactoryAsync()
    {
        await _container.StartAsync();

        Factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll<MongoContext>();
                    services.RemoveAll<IOptions<MongoSettings>>();
                    services.AddSingleton(new MongoContext(Options.Create(new MongoSettings
                    {
                        ConnectionString = ConnectionString,
                        DatabaseName = DatabaseName
                    })));
                });
            });
    }

    public async Task DisposeAsync()
    {
        if (Factory is not null)
        {
            await Factory.DisposeAsync();
        }

        await _container.DisposeAsync();
    }
}
