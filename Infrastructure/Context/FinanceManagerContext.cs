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

        public FinanceManagerContext(DbContextOptions<FinanceManagerContext> options) : base(options) { }
    }
}
