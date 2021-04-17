using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccess.DataModel
{
    [Table("Stock")]
   public class Stock
    {
        [Key]
        public int StockId { get; set; }
        public int ProductId { get; set; }
        public int AvailableQuantity { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
