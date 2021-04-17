using BusinessAccess.RequestModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccess.Interfaces
{
    public interface ICategoryService
    {
        Task<ApiResponse> Add(AddCategoryRequestModel options);
        Task<ApiResponse> GetCategories(GetCategoriesRequestModel options);
        Task<ApiResponse> GetCategory(int catId);
        Task<ApiResponse> Update(int cateId,AddCategoryRequestModel options);
        Task<ApiResponse> UpdateStatus(int cateId, UpdateStatusRequestModel options);
        Task<ApiResponse> Delete(int catId);
    }
}
