using System.Data.Entity;

namespace ProFIT
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("DefaultConnection")
        {
        }

        public DbSet<FitnesData> FitnesDatas { get; set; }
    }
}
