using FinanceManager.Infrastructure.Model;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Infrastructure.Context
{
    public class FinanceManagerContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<MonthlyBalance> MonthlyBalances { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder
                .UseMySql("server=localhost;database=FinanceManager;user=finance_manager;password=1337", new MySqlServerVersion("8.0.21"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
