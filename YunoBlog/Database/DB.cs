using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace YunoBlog.DataBase
{
    public class DB : DbContext
    {
        public DbSet<Models.Category> Categories { get; set; }
        public DbSet<Models.Content> Contents { get; set; }
        public DbSet<Models.Link> Links { get; set; }
        public DB()
            : base("mysqldb")
        { 
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Category>()
                .HasMany(c => c.Contents)
                .WithOptional(c => c.Category);
        }
    }
}