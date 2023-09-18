using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Dashboard.Application.Services;

namespace Dashboard.Application
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(typeof(DependencyInjection).Assembly);
            services.AddHostedService<ConsumerService>();

            return services;
        }
    }
}
