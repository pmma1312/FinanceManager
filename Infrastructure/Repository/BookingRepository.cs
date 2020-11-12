using FinanceManager.Infrastructure.Context;
using FinanceManager.Infrastructure.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManager.Infrastructure.Repository
{
    public interface IBookingRepository : IRepository<Booking>
    {
        public Task<List<Booking>> GetBookingsWithCategory(long categoryId);
        public Task<List<Booking>> GetBookingsForMonth(DateTime date);
        public Task<List<Booking>> GetBookingsForUser(long userId);
        public Task<List<Booking>> GetBookingsWithCategory(long categoryId, long userId);
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
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Booking>> GetBookingsWithCategory(long categoryId, long userId)
        {
            return await _context.Bookings
                .Where(booking => booking.BookingCategory.CategoryId == categoryId
                    && booking.BookingUser.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Booking>> GetBookingsForMonth(DateTime date)
        {
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            return await _context.Bookings
                .Where(booking => booking.BookingDate > firstDayOfMonth && booking.BookingDate < lastDayOfMonth)
                .ToListAsync();
        }

        public async Task<List<Booking>> GetBookingsForUser(long userId)
        {
            return await _context.Bookings
                .Where(booking => booking.BookingUser.UserId == userId)
                .AsNoTracking()
                .Include(booking => booking.BookingCategory)
                    .AsNoTracking()
                .ToListAsync();
        }
    }
}
