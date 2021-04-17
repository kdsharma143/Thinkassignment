using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BusinessAccess.RequestModel
{
    public class TransactionRequestModel
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Select Product")]
        public int ProductId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select type")]
        public string Type { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be more than 0")]
        public int Quantity { get; set; }
    }

    public class GetTransactionsRequestModel:BaseRequestModel
    {
        public string Search { get; set; }
        public int CategoryId { get; set; }
        public string Type { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class GetStockRepotRequestModel:BaseRequestModel
    {
        public int CategoryId { get; set; }
    }
}
