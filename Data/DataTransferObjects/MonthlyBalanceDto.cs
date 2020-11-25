using System;

namespace FinanceManager.Data.DataTransferObjects
{
    public class MonthlyBalanceDto
    {
        public long? MonthlyBalanceId { get; set; }
        public DateTime ValidUntil { get; set; }
        public float AvailableMonthlyBalance { get; set; } 
    }
}
