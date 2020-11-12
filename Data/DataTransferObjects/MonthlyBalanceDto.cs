using System;

namespace FinanceManager.Data.DataTransferObjects
{
    public class MonthlyBalanceDto
    {
        public DateTime ValidUntil { get; set; }
        public float AvailableMonthlyBalance { get; set; }
    }
}
