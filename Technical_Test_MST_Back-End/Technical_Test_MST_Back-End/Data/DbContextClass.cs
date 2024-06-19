using Technical_Test_MST_Back_End.Models;
using Microsoft.EntityFrameworkCore;

namespace Technical_Test_MST_Back_End.Data
{
    public class DbContextClass : DbContext
    {
        public DbContextClass(DbContextOptions<DbContextClass> options)
            : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
    }
}
