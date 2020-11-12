using FinanceManager.Data.DataTransferObjects;
using FluentValidation;

namespace FinanceManager.Infrastructure.Validation
{
    public class CategoryDtoValidator : AbstractValidator<CategoryDto>
    {

        public CategoryDtoValidator()
        {
            RuleFor(category => category.CategoryName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(14);
        }

    }
}
