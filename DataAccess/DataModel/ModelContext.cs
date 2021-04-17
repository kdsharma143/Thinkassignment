using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DataModel
{
    public class ModelContext :DbContext
    {
        public ModelContext(DbContextOptions<ModelContext> options)
           :base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<Stock> Stocks { get; set; }
    }
}
