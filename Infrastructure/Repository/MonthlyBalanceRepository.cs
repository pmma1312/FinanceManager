using FinanceManager.Infrastructure.Context;
using FinanceManager.Infrastructure.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManager.Infrastructure.Repository
{
    public interface IMonthlyBalanceRepository : IRepository<MonthlyBalance>
    {
        public Task<MonthlyBalance> GetNewestMonthlyBalance(long userId);
    }

    public class MonthlyBalanceRepository : GenericRepository<MonthlyBalance>, IMonthlyBalanceRepository
    {
        public MonthlyBalanceRepository(FinanceManagerContext context) : base(context) { }

        public async Task<MonthlyBalance> GetNewestMonthlyBalance(long userId)
        {
            return await _context.MonthlyBalances
                .Where(balance => balance.BalanceUser.UserId == userId)
                .OrderByDescending(balance => balance.MonthylBalanceId)
                .FirstOrDefaultAsync();
        }
    }
}
