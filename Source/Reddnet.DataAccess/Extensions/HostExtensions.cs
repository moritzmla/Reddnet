using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Reddnet.DataAccess.Extensions;

public static class HostExtensions
{
    public static IHost InitializeDatabase(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        context.Database.EnsureCreated();
        return host;
    }
}
