using FinanceManager.Data.DataTransferObjects;
using FluentValidation;
using System;

namespace FinanceManager.Infrastructure.Validation
{
    public class MonthlyBalanceDtoValidator : AbstractValidator<MonthlyBalanceDto>
    {

        public MonthlyBalanceDtoValidator()
        {
            RuleFor(balance => balance.AvailableMonthlyBalance)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(balance => balance.ValidUntil)
                .NotEmpty()
                .GreaterThan(DateTime.Now);
        }

    }
}
