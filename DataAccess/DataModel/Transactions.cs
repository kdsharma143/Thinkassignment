using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccess.DataModel
{
    [Table("Transactions")]
   public class Transactions
    {
        [Key]
        public int TransactionId { get; set; }
        public int ProductId { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
