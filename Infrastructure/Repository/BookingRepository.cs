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
        public Task<List<Booking>> GetBookingsForMonth(DateTime date, long userId);
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

        public async Task<List<Booking>> GetBookingsForMonth(DateTime date, long userId)
        {
            var lastDayOfMonth = new DateTime(date.Year, date.Month, 25);
            var firstDayOfMonth = lastDayOfMonth.AddMonths(-1).AddDays(-1);

            return await _context.Bookings
                .Where(booking => booking.BookingDate > firstDayOfMonth 
                    && booking.BookingDate < lastDayOfMonth 
                    && booking.BookingUser.UserId == userId)
                    .Include(booking => booking.BookingCategory)
                    .AsNoTracking()
                .OrderByDescending(booking => booking.BookingDate)
                .ToListAsync();
        }

        public async Task<List<Booking>> GetBookingsForUser(long userId)
        {
            return await _context.Bookings
                .Where(booking => booking.BookingUser.UserId == userId)
                .OrderByDescending(booking => booking.BookingDate)
                .AsNoTracking()
                .Include(booking => booking.BookingCategory)
                    .AsNoTracking()
                .ToListAsync();
        }
    }
}
