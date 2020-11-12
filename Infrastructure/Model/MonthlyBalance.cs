using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManager.Infrastructure.Model
{
    public class MonthlyBalance
    {
        [Key]
        public long MonthylBalanceId { get; set; }
        [ForeignKey("UserId")]
        public User BalanceUser { get; set; }
        public DateTime ValidUntil { get; set; }
        public float AvailableMonthlyBalance { get; set; }
    }
}
