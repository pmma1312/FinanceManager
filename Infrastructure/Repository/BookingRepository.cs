using FinanceManager.Infrastructure.Context;
using FinanceManager.Infrastructure.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManager.Infrastructure.Repository
{
    public interface IBookingRepository : IRepository<Booking>
    {
        public Task<List<Booking>> GetBookingsWithCategory(long categoryId);
    }

    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(FinanceManagerContext context) : base(context)
        {

        }

        public async Task<List<Booking>> GetBookingsWithCategory(long categoryId)
        {
            return await _context.Bookings
                .Where(booking => booking.BookingCategory.CategoryId == categoryId)
                .ToListAsync();
        }
    }
}
