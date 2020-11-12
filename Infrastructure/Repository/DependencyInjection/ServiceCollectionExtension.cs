using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Infrastructure.Repository.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IMonthlyBalanceRepository, MonthlyBalanceRepository>();

            return services;
        }
    }
}
