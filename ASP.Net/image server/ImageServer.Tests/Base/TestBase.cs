using ImageServer.Api.Data;
using ImageServer.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ImageServer.Tests.Base;

public abstract class TestBase : IDisposable
{
    protected readonly ImageDbContext DbContext;
    protected readonly IServiceProvider ServiceProvider;

    protected TestBase()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();
        DbContext = ServiceProvider.GetRequiredService<ImageDbContext>();
    }

    protected virtual void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ImageDbContext>(options =>
            options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

        services.AddMemoryCache();
        services.AddLogging();
        services.AddHttpContextAccessor();
    }

    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Dispose();
    }
}
