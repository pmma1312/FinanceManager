using FinanceManager.Data.DataTransferObjects;
using FluentValidation;

namespace FinanceManager.Infrastructure.Validation
{
    public class BookingDtoValidator : AbstractValidator<BookingDto>
    {

        public BookingDtoValidator()
        {
            RuleFor(booking => booking.BookingAmount)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(booking => booking.BookingCategoryId)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(booking => booking.BookingDescription)
                .MinimumLength(3)
                .When(booking => booking.BookingDescription != null);
        }

    }
}
