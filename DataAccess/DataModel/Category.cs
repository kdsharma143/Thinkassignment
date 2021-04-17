using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccess.DataModel
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(30)]
        public string CatCode { get; set; }
        public bool  IsActive { get; set; }
        public bool IsRemoved { get; set; }
        public DateTime LastUpdated { get; set; }

    }
}
