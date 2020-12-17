using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Reddnet.Application.Behaviours;
using System.Reflection;

namespace Reddnet.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            services.AddMediatR(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
