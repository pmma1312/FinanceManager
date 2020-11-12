using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Service.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddFinanceManagerServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRequestDataService, RequestDataService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IMonthlyBalanceService, MonthlyBalanceService>();
            services.AddScoped<IBookingService, BookingService>();

            return services;
        }
    }
}
