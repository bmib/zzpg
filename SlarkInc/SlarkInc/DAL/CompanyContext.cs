using System.Data.Entity;
using SlarkInc.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace SlarkInc.DAL
{
    public class CompanyContext : DbContext
    {
        public CompanyContext()  : base("CompanyContext")
        { 
        }

        public DbSet<Worker> Workers {get;set;}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}