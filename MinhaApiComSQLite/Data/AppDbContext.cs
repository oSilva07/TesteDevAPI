using Microsoft.EntityFrameworkCore;
using MinhaApiComSQLite.Models;

namespace MinhaApiComSQLite.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<HistoricoPreco> HistoricoPrecos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Produto>()
                .HasIndex(p => p.Nome)
                .IsUnique();

            modelBuilder.Entity<Categoria>()
                .HasIndex(c => c.Nome)
                .IsUnique();
        }
    }
}
