using FinanceManager.Data.DataTransferObjects;
using FinanceManager.Data.Response;
using FinanceManager.Infrastructure.Model;
using FinanceManager.Infrastructure.Repository;
using FinanceManager.Infrastructure.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FinanceManager.Service
{
    public interface ICategoryService
    {
        public Task<BaseResponse> Create(CategoryDto category);
        public Task<BaseResponse> GetCategoriesForUser();
        public Task<BaseResponse> DeleteCategory(long categoryId);
    }
    
    public class CategoryService : ICategoryService
    {
        private ICategoryRepository _categoryRepository;
        private IRequestDataService _requestDataService;
        private IBookingRepository _bookingRepository;

        public CategoryService(ICategoryRepository categoryRepository, IRequestDataService requestDataService, IBookingRepository bookingRepository)
        {
            _categoryRepository = categoryRepository;
            _requestDataService = requestDataService;
            _bookingRepository = bookingRepository;
        }

        public async Task<BaseResponse> GetCategoriesForUser()
        {
            var response = new BaseResponse();
            
            User user = await _requestDataService.GetCurrentUser();
            
            response.Data.Add("categories", await _categoryRepository.SelectCategoriesForUserWithId(user.UserId));

            return response;
        }

        public async Task<BaseResponse> DeleteCategory(long categoryId)
        {
            var response = new BaseResponse();

            User currentUser = await _requestDataService.GetCurrentUser();

            var dbCategory = await _categoryRepository.GetById(categoryId);

            if(dbCategory is null)
            {
                response.Infos.Errors.Add($"Category with id {categoryId} doesn't exist.");
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }

            if(currentUser.UserId != dbCategory.CategoryOwner.UserId)
            {
                response.Infos.Errors.Add($"Category with id {categoryId} doesn't belong to your account. You can only delete your own categories.");
                response.StatusCode = HttpStatusCode.Unauthorized;
                return response;
            }

            var bookings = await _bookingRepository.GetBookingsWithCategory(categoryId);

            if(bookings.Count > 0)
            {
                response.Infos.Errors.Add($"Category {dbCategory.CategoryName} can't be deleted because bookings exist that reference this category");
                response.StatusCode = HttpStatusCode.UnprocessableEntity;
                return response;
            }

            await _categoryRepository.Delete(dbCategory);

            return response;
        }

        public async Task<BaseResponse> Create(CategoryDto category)
        {
            var response = new BaseResponse();

            CategoryDtoValidator validator = new CategoryDtoValidator();
            var result = validator.Validate(category);

            if(!result.IsValid)
            {
                response.Infos.Errors.AddRange(result.Errors.ToList().Select(error => error.ErrorMessage));
                response.StatusCode = HttpStatusCode.UnprocessableEntity;
                return response;
            }

            User currentUser = await _requestDataService.GetCurrentUser();

            var dbCategory = await _categoryRepository.SelectCategoryWithUidAndName(currentUser.UserId, category.CategoryName);

            if(dbCategory != null)
            {
                response.Infos.Errors.Add($"A category with the name {dbCategory.CategoryName} exists already.");
                response.StatusCode = HttpStatusCode.Conflict;
                return response;
            }

            var newCategory = new Category
            {
                CategoryName = category.CategoryName,
                CategoryOwner = currentUser
            };

            newCategory = await _categoryRepository.Insert(newCategory);
            newCategory.CategoryOwner = null;

            response.Data.Add("category", newCategory);

            return response;
        }

    }
}
