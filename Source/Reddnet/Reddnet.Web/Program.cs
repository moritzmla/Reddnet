using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Reddnet.Application;
using Reddnet.Application.Interfaces;
using Reddnet.DataAccess;
using Reddnet.DataAccess.Extensions;
using Reddnet.Web.Authorization;

namespace Reddnet.Web
{
    public static class Program
    {
        public static void Main(string[] args)
            => CreateHostBuilder(args)
                .Build()
                .InitializeDatabase()
                .Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddApplication();
                    services.AddDataAccess(context.Configuration.GetConnectionString("DefaultConnection"));
                    services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
                });
    }
}
