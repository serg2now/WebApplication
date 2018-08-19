using System.Data.Entity;
using WebApplicationExercise.Models.EFModels;

namespace WebApplicationExercise.Core
{
    public class MainDataContext : DbContext
    {
        public DbSet<DbOrder> Orders { get; set; }

        public DbSet<DbProduct> Products { get; set; }

        public MainDataContext()
        {
            Database.SetInitializer(new CustomDataBaseInitializer());
        }
    }
}