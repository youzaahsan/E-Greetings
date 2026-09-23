using E_Greetings.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_Greetings.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
    }
}
