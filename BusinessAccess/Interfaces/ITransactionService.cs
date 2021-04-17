using BusinessAccess.RequestModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccess.Interfaces
{
   public interface ITransactionService
    {
        Task<ApiResponse> StockTansaction(TransactionRequestModel options);
        Task<ApiResponse> GetTransactions(GetTransactionsRequestModel options);
        Task<ApiResponse> GetStockRepot(GetStockRepotRequestModel options);
    }
}
