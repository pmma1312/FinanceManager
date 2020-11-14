using FinanceManager.Data.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Infrastructure.Model
{
    public class Booking
    {
        [Key]
        public long BookingId { get; set; }
        [ForeignKey("CategoryId")]
        public Category BookingCategory { get; set; }
        [ForeignKey("UserId")]
        public User BookingUser { get; set; }
        public DateTime BookingDate { get; set; }
        public float BookingAmount { get; set; }
        public BookingTypeEnum BookingType { get; set; }
        public string? BookingDescription { get; set; }
    }
}
