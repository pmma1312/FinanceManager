using FinanceManager.Infrastructure.Context;
using FinanceManager.Infrastructure.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManager.Infrastructure.Repository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        public Task<Category> SelectCategoryWithUidAndName(long userId, string name);
        public Task<List<Category>> SelectCategoriesForUserWithId(long userId);
    }

    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(FinanceManagerContext context) : base(context)
        {

        }

        public async Task<Category> SelectCategoryWithUidAndName(long userId, string name)
        {
            return await _context.Categories
                .Where(category => category.CategoryOwner.UserId == userId && category.CategoryName == name)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Category>> SelectCategoriesForUserWithId(long userId)
        {
            return await _context.Categories
                .Where(category => category.CategoryOwner.UserId == userId)
                    .AsNoTracking()
                .ToListAsync();
        }
    }
}
