using FinanceManager.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Infrastructure.Repository.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDatabaseDependencyInjection(this IServiceCollection services)
        {
            services.AddDbContext<FinanceManagerContext>(options => {
                options.UseMySQL("server=localhost;database=FinanceManager;user=finance_manager;password=1337");
            });

            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
