using Microsoft.EntityFrameworkCore;

namespace Assement.Models
{
    public class DBContext:DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }

        public DbSet<Product> products { get; set; }
        public DbSet<Genre> Genres { get; set; }



    }
}
