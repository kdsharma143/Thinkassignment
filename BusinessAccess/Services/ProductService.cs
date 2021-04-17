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
    public class ProductService : IDisposable, IProductService
    {
        private ModelContext _context;
        public ProductService(ModelContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse> Add(AddProductRequestModel options)
        {
            ApiResponse response = new ApiResponse();

            var category = await _context.Categories.FindAsync(options.CategoryId);
            if (category == null)
            {
                response.Status = 0;
                response.Message = $"No category found with id: {options.CategoryId}";
                return response;
            }

            var product = await _context.Products.FirstOrDefaultAsync(x => x.CategoryId == options.CategoryId && x.Name == options.Name);
            if (product != null)
            {
                response.Status = 0;
                response.Message = $"{options.Name} already exists.";
                return response;
            }

            var count = await _context.Products.CountAsync();
            Product objProdcut = new Product
            {
                CategoryId = options.CategoryId,
                Description = options.Description,
                IsActive = true,
                IsRemoved = false,
                LastUpdated = DateTime.Now,
                Name = options.Name,
                Price = options.Price,
                ProductCode = $"{category.Name.Substring(0, 3).ToUpper()}-{options.Name.Substring(0, 3).ToUpper()}-{count + 1}",
            };

            await _context.Products.AddAsync(objProdcut);
            await _context.SaveChangesAsync();

            Stock objStock = new Stock
            {
                ProductId = objProdcut.ProductId,
                AvailableQuantity = 0,
                LastUpdated = DateTime.Now
            };

            await _context.Stocks.AddAsync(objStock);
            await _context.SaveChangesAsync();

            response.Status = 1;
            response.Message = $"{options.Name} saved successfully.";

            return response;
        }
        public async Task<ApiResponse> Update(int productId, AddProductRequestModel options)
        {
            ApiResponse response = new ApiResponse();
            var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
            if (product == null)
            {
                response.Status = 0;
                response.Message = $"No product found with id: {productId}";
                return response;
            }

            var category = await _context.Categories.FindAsync(options.CategoryId);
            if (category == null)
            {
                response.Status = 0;
                response.Message = $"No category found with id: {options.CategoryId}";
                return response;
            }

            var productCheck = await _context.Products.FirstOrDefaultAsync(x => x.CategoryId == options.CategoryId && x.Name == options.Name && x.ProductId != productId);
            if (productCheck != null)
            {
                response.Status = 0;
                response.Message = $"{options.Name} already exists.";
                return response;
            }

            product.CategoryId = options.CategoryId;
            product.Name = options.Name;
            product.Price = options.Price;
            product.Description = options.Description;

            await _context.SaveChangesAsync();

            response.Status = 1;
            response.Message = $"{options.Name} updated successfully.";

            return response;
        }
        public async Task<ApiResponse> GetProducts(GetProductRequestModel options)
        {
            ApiResponse response = new ApiResponse();
            int _pageNo = (options.pageNo == 0) ? 1 : options.pageNo;
            int _pageLimit = (options.pageSize == 0) ? int.MaxValue : options.pageSize;
            var products = from prod in _context.Products
                           join cat in _context.Categories
                           on prod.CategoryId equals cat.CategoryId
                           where prod.IsActive == true && prod.IsRemoved == false
                           select new
                           {
                               Category = cat.Name,
                               Product = prod.Name,
                               prod.ProductId,
                               prod.ProductCode,
                               prod.Description,
                               prod.LastUpdated,
                               prod.IsActive,
                               prod.IsRemoved,
                               prod.Price
                           };

            if (!String.IsNullOrEmpty(options.Name))
            {
                products = products.Where(x => x.Product.Contains(options.Name));
            }

            if (!String.IsNullOrEmpty(options.CategoryName))
            {
                products = products.Where(x => x.Category.Contains(options.CategoryName));
            }

            if (!String.IsNullOrEmpty(options.ProductCode))
            {
                products = products.Where(x => x.ProductCode.Contains(options.ProductCode));
            }
            if (options.Price.HasValue)
            {
                products = products.Where(x => x.Price == options.Price);
            }

            response.Data = await products.Skip((_pageNo - 1) * _pageLimit).Take(_pageLimit).ToListAsync();
            response.Status = 1;
            response.Message = "Category get successfully";
            return response;
        }
        public async Task<ApiResponse> GetProduct(int productId)
        {
            ApiResponse response = new ApiResponse();
            var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
            if (product != null)
            {
                response.Data = product;
                response.Status = 1;
                response.Message = "Category get successfully";

            }
            else
            {
                response.Status = 0;
                response.Message = $"No product found with id: {productId}";
            }
            return response;
        }
        public async Task<ApiResponse> Delete(int productId)
        {
            ApiResponse response = new ApiResponse();

          

            var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
            if (product == null)
            {
                response.Message = $"No product found with id : {productId}";
                return response;
            }

            var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductId == productId);
            if(stock.AvailableQuantity>0)
            {
                response.Message = $"{product.Name} has stock in hand so it cant delete.";
                return response;
            }

            product.LastUpdated = DateTime.Now;
            product.IsRemoved = true;

            await _context.SaveChangesAsync();
            response.Message = $"{product.Name} removed successfully";
            response.Status = 1;

            return response;
        }
        public async Task<ApiResponse> UpdateStatus(int productId, UpdateStatusRequestModel options)
        {
            ApiResponse response = new ApiResponse();

            var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
            if (product == null)
            {
                response.Message = $"No product found with id : {productId}";
                return response;
            }

            product.LastUpdated = DateTime.Now;
            product.IsActive = options.Status;

            await _context.SaveChangesAsync();
            response.Message = $"{product.Name} updated successfully";
            response.Status = 1;

            return response;
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
