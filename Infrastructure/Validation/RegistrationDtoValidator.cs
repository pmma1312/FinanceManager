using FinanceManager.Data.DataTransferObjects;
using FluentValidation;

namespace FinanceManager.Infrastructure.Validation
{
    public class RegistrationDtoValidator : AbstractValidator<RegistrationDto>
    {
        public RegistrationDtoValidator()
        {
            RuleFor(user => user.Username)
                .NotEmpty()
                .MaximumLength(12)
                .MinimumLength(1);
            
            RuleFor(user => user.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(user => user.Password)
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(64);
        }
    }
}
