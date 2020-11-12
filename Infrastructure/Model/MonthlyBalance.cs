using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManager.Infrastructure.Model
{
    public class MonthlyBalance
    {
        public long MonthylBalanceId { get; set; }
        public User BalanceUser { get; set; }
        public DateTime ValidUntil { get; set; }
        public float AvailableMonthlyBalance { get; set; }
    }
}
