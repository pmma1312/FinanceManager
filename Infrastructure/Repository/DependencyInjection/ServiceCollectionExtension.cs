using FinanceManager.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace FinanceManager.Infrastructure.Repository.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDatabaseDependencyInjection(this IServiceCollection services)
        {
            services.AddDbContext<FinanceManagerContext>();

            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
