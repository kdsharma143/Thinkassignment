using BusinessAccess.RequestModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccess.Interfaces
{
    public interface IProductService
    {
        Task<ApiResponse> Add(AddProductRequestModel options);
        Task<ApiResponse> Update(int productId, AddProductRequestModel options);
        Task<ApiResponse> GetProducts(GetProductRequestModel options);
        Task<ApiResponse> GetProduct(int productId);
        Task<ApiResponse> Delete(int productId);
        Task<ApiResponse> UpdateStatus(int productId, UpdateStatusRequestModel options);

    }
}
