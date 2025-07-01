using Microsoft.EntityFrameworkCore;
using MyApp.Entities;

namespace MyApp.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Artículo> Artículos { get; set; }
        public DbSet<Préstamo> Préstamos { get; set; }


    }
}
