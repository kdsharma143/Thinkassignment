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
    public class TransactionService : IDisposable, ITransactionService
    {
        private ModelContext _context;
        public TransactionService(ModelContext context)
        {
            _context = context;
        }
        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<ApiResponse> StockTansaction(TransactionRequestModel options)
        {
            ApiResponse response = new ApiResponse();
            List<string> TypeList = new List<string> { "Add", "Sale" };
            if(!TypeList.Contains(options.Type))
            {
                response.Message = $"type will be Add or Sale only.";
                return response;
            }

            var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductId == options.ProductId);
            if(stock==null)
            {
                response.Message = $"No stock detail find for id: {options.ProductId}.";
                return response;
            }

            if (options.Type == "Add")
            {
                stock.AvailableQuantity += options.Quantity;
            }
            else if (options.Type == "Sale")
            {
                if (stock.AvailableQuantity < options.Quantity)
                {
                    response.Message = $"Available quantity is less then sold quantity.";
                    return response;
                }
                else
                {
                    stock.AvailableQuantity -= options.Quantity;
                }
            }
            stock.LastUpdated = DateTime.Now;
            Transactions objTrans = new Transactions
            {
                ProductId = options.ProductId,
                Type = options.Type,
                Quantity = options.Quantity,
                TransactionDate = DateTime.Now,
            };

            await _context.Transactions.AddAsync(objTrans);
            await _context.SaveChangesAsync();

            response.Message = $"Transaction compleated.";
            response.Status = 1;

            return response;
        }

        public async Task<ApiResponse> GetTransactions(GetTransactionsRequestModel options)
        {
            ApiResponse response = new ApiResponse();

            int _pageNo = (options.pageNo == 0) ? 1 : options.pageNo;
            int _pageLimit = (options.pageSize == 0) ? int.MaxValue : options.pageSize;

            var transactions = from trans in _context.Transactions
                               join prod in _context.Products on trans.ProductId equals prod.ProductId
                               join cat in _context.Categories on prod.CategoryId equals cat.CategoryId
                               select new
                               {
                                   TransactionDate = trans.TransactionDate,
                                   Quantity = trans.Quantity,
                                   Type = trans.Type,
                                   TransactionId = trans.TransactionId,
                                   ProductName = prod.Name,
                                   CategoryName = cat.Name,
                                   CategoryCode = cat.CatCode,
                                   ProductCode = prod.ProductCode,
                                   CategoryId= cat.CategoryId
                               };

            if(options.CategoryId>0)
            {
                transactions = transactions.Where(x => x.CategoryId == options.CategoryId);
            }

            if(!string.IsNullOrEmpty(options.Type))
            {
                transactions = transactions.Where(x => x.Type == options.Type.Trim());
            }

            if(options.StartDate.HasValue)
            {
                transactions = transactions.Where(x => x.TransactionDate >= options.StartDate);
            }

            if (options.EndDate.HasValue)
            {
                transactions = transactions.Where(x => x.TransactionDate <= options.EndDate);
            }

            if(!string.IsNullOrEmpty(options.Search))
            {
                transactions = transactions.Where(x => x.ProductName.Contains(options.Search) ||
                                                     x.ProductCode.Contains(options.Search) ||
                                                     x.CategoryCode.Contains(options.Search) ||
                                                     x.CategoryName.Contains(options.Search));
            }

            response.Data = await transactions.Skip((_pageNo - 1) * _pageLimit).Take(_pageLimit).ToListAsync();
            response.Message = "Transactions get successfully";
            response.Status = 1;

            return response;
        }

        public async Task<ApiResponse>GetStockRepot(GetStockRepotRequestModel options)
        {
            ApiResponse response = new ApiResponse();

            int _pageNo = (options.pageNo == 0) ? 1 : options.pageNo;
            int _pageLimit = (options.pageSize == 0) ? int.MaxValue : options.pageSize;

            var stocks = from stock in _context.Stocks
                         join prod in _context.Products on stock.ProductId equals prod.ProductId
                         join cat in _context.Categories on prod.CategoryId equals cat.CategoryId
                         select new
                         {
                             ProductName = prod.Name,
                             CategoryName = cat.Name,
                             CategoryCode = cat.CatCode,
                             ProductCode = prod.ProductCode,
                             CategoryId = cat.CategoryId,
                             AvailableQuantity= stock.AvailableQuantity,
                             LastUpdated= stock.LastUpdated
                         };

            if(options.CategoryId>0)
            {
                stocks = stocks.Where(x => x.CategoryId == options.CategoryId);
            }

            response.Data = await stocks.Skip((_pageNo - 1) * _pageLimit).Take(_pageLimit).ToListAsync();
            response.Message = "Stock report get successfully";
            response.Status = 1;

            return response;
        }
    }
}
