using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using Ticketing.Infrastructure.Persistence;

namespace Ticketing.Infrastructure
{
    public static class DependencyInjectionInfrastructure
    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) 
        {
            //add db connection
            var cs = configuration.GetConnectionString("Ticketing") ?? throw new InvalidOperationException("Miss ConnectionString : Tickting");
            services.AddDbContext<TicketingDbContext>(options => options.UseSqlServer(cs));

            // add more repositories
            // // services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }
    }
}
