using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccess.DataModel
{
    [Table("Product")]
   public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        [StringLength(20)]
        public string ProductCode { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(2000)]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public bool IsRemoved { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
