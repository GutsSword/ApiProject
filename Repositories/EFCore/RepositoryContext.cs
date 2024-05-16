using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repositories.EFCore.Config;
using System.Reflection;

namespace Repositories.EFCore
{
    public class RepositoryContext : IdentityDbContext<User>
    {
        public RepositoryContext(DbContextOptions options) :
            base(options)
        {

        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }

        //Migration sırasında model oluşturulurken BookConfig sınıfındaki Seedleme dikkate alınacak.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);   // Added after Identity implentation for consistent migraiton creating.
            //modelBuilder.ApplyConfiguration(new BookConfig()); // Seeding.
            //modelBuilder.ApplyConfiguration(new RoleConfiguration()); // Seeding.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
