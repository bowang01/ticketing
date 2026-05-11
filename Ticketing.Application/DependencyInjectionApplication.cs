using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.Design;

namespace Ticketing.Application
{
    public static class DependencyInjectionApplication
    {

        public static IServiceCollection AddApplication(this IServiceCollection services) 
        {
            // add more services


            return services;
        }
    }
}
