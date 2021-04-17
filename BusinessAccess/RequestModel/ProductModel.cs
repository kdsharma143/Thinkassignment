using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BusinessAccess.RequestModel
{
    public class AddProductRequestModel
    {
        [Required]
        [Range(1,int.MaxValue,ErrorMessage ="Select category")]
        public int CategoryId { get; set; }
        [Required(AllowEmptyStrings =false,ErrorMessage ="Enter product name")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter product description")]
        public string Description { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter price")]
        public decimal Price { get; set; }
    }


    public class GetProductRequestModel : BaseRequestModel
    {
        public string Name { get; set; }
        public string  ProductCode { get; set; }
        public decimal? Price { get; set; }
        public string  CategoryName { get; set; }
    }

}
