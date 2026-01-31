using Microsoft.EntityFrameworkCore;
using PokeAPI.Models;

namespace PokeAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Pokemon> Pokemon { get; set; }
        public DbSet<PokemonType> PokemonTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar que el Id NO se genera automáticamente
            modelBuilder.Entity<Pokemon>()
                .Property(p => p.Id)
                .ValueGeneratedNever(); // ← ESTO ES LO IMPORTANTE

            // Configurar relación Pokemon - PokemonTypes
            modelBuilder.Entity<Pokemon>()
                .HasMany(p => p.Types)
                .WithOne(t => t.Pokemon)
                .HasForeignKey(t => t.PokemonId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configurar índice único para nombre
            modelBuilder.Entity<Pokemon>()
                .HasIndex(p => p.Name)
                .IsUnique();
        }

    }
}
