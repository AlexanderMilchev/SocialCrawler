using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twitter.Models;

namespace Twitter.DAL
{
    public class TwitterDbContext : DbContext
    {

        public TwitterDbContext() : base("MyDbContextConnectionString")
        {
           
        }

        public DbSet<Status> t_TwitterResults { get; set; }
      

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}
