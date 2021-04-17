using BusinessAccess.Interfaces;
using BusinessAccess.RequestModel;
using DataAccess.DataModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccess.Services
{
    public class CategoryService : IDisposable, ICategoryService
    {
        private ModelContext _context;
        public CategoryService(ModelContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse> Add(AddCategoryRequestModel options)
        {
            ApiResponse response = new ApiResponse();

            var category = await _context.Categories.Where(x => x.Name == options.Name).FirstOrDefaultAsync();
            if (category != null)
            {
                response.Message = $"{options.Name} already exists.";
                return response;
            }
            int catCount = await _context.Categories.CountAsync();
            Category objCat = new Category
            {
                IsActive = true,
                CatCode = $"{ options.Name.Substring(0, 3).ToUpper()}-{catCount + 1}",
                IsRemoved = false,
                LastUpdated = DateTime.Now,
                Name = options.Name
            };

            await _context.Categories.AddAsync(objCat);
            await _context.SaveChangesAsync();

            response.Message = $"{options.Name} saved successfully";
            response.Status = 1;

            return response;
        }

        public async Task<ApiResponse> GetCategories(GetCategoriesRequestModel options)
        {
            ApiResponse response = new ApiResponse();
            int _pageNo = (options.pageNo==0) ? 1: options.pageNo;
            int _pageLimit = (options.pageSize == 0) ? int.MaxValue : options.pageSize;
            var categories = _context.Categories.AsQueryable();

            if (!String.IsNullOrEmpty(options.name)){
                categories = categories.Where(x => x.Name.Contains(options.name));
            }

            response.Data = await categories.Skip((_pageNo - 1) * _pageLimit).Take(_pageLimit).ToListAsync();
            response.Status = 1;
            response.Message = "Category get successfully";
            return response;
        }


        public async Task<ApiResponse> GetCategory(int catId)
        {
            ApiResponse response = new ApiResponse();
            var categories = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == catId);
            if (categories != null)
            {
                response.Data = categories;
                response.Status = 1;
                response.Message = "Category get successfully";
               
            }
            else
            {
                response.Status = 0;
                response.Message = $"No Category found with id: {catId}";
            }
            return response;
        }


        public async Task<ApiResponse> Update(int catId,AddCategoryRequestModel options)
        {
            ApiResponse response = new ApiResponse();

            var category = await _context.Categories.FirstOrDefaultAsync(x=>x.CategoryId== catId);
            if (category == null)
            {
                response.Message = $"No category found with id : {catId}";
                return response;
            }

            category.LastUpdated = DateTime.Now;
            category.Name = options.Name;

            await _context.SaveChangesAsync();
            response.Message = $"{options.Name} updated successfully";
            response.Status = 1;

            return response;
        }

        public async Task<ApiResponse> Delete(int catId)
        {
            ApiResponse response = new ApiResponse();

            var category = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == catId);
            if (category == null)
            {
                response.Message = $"No category found with id : {catId}";
                return response;
            }

            var products = await _context.Products.Where(x => x.CategoryId == catId && x.IsRemoved == false).ToListAsync();
            if(products.Count>0)
            {
                response.Message = $"Category has {products.Count} active products so we cant delete.";
                return response;
            }

            category.LastUpdated = DateTime.Now;
            category.IsRemoved = true;

            await _context.SaveChangesAsync();
            response.Message = $"{category.Name} removed successfully";
            response.Status = 1;

            return response;
        }

        public async Task<ApiResponse> UpdateStatus(int catId, UpdateStatusRequestModel options)
        {
            ApiResponse response = new ApiResponse();

            var category = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == catId);
            if (category == null)
            {
                response.Message = $"No category found with id : {catId}";
                return response;
            }

            category.LastUpdated = DateTime.Now;
            category.IsActive = options.Status;

            await _context.SaveChangesAsync();
            response.Message = $"{category.Name} updated successfully";
            response.Status = 1;

            return response;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
