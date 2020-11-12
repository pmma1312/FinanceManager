using System;

namespace FinanceManager.Infrastructure.Model
{
    public class Booking
    {
        public long BookingId { get; set; }
        public Category BookingCategory { get; set; }
        public User BookingUser { get; set; }
        public DateTime BookingDate { get; set; }
        public float BookingAmount { get; set; }
        public string? BookingDescription { get; set; }
    }
}
