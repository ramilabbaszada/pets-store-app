using Core.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccesss.Concrete.Contexts
{
    public class TestProjectContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=PetStore;Trusted_Connection=true;Encrypt=False");
        }
        public DbSet<User> Users { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
    }
}
