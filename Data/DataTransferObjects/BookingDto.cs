namespace FinanceManager.Data.DataTransferObjects
{
    public class BookingDto
    {
        public long BookingCategoryId { get; set; }
        public float BookingAmount { get; set; }
        public string BookingDescription { get; set; }
    }
}
