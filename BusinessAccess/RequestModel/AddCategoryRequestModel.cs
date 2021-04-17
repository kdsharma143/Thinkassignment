using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BusinessAccess.RequestModel
{
   public class AddCategoryRequestModel
    {
        [Required (AllowEmptyStrings =false,ErrorMessage ="Category Name Required")]
        public string Name { get; set; }
    }

    public class GetCategoriesRequestModel: BaseRequestModel
    {
        public string name { get; set; }
    }

    public class UpdateStatusRequestModel
    {
        [Required(AllowEmptyStrings =false,ErrorMessage ="Empty Status")]
        public bool Status { get; set; }
    }
}
