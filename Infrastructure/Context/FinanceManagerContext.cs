using FinanceManager.Infrastructure.Model;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Infrastructure.Context
{
    public class FinanceManagerContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<MonthlyBalance> MonthlyBalances { get; set; }
        
        public FinanceManagerContext(DbContextOptions<FinanceManagerContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

    }
}
