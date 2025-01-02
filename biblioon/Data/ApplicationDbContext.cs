using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using biblioon.Models;

namespace biblioon.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<EdiLivro> EdiLivros { get; set; }
        public DbSet<Editor> Editores { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Biblioteca> Biblioteca { get; set; }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<UniLivro> UniLivros { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EdiLivro>()
                .HasMany(e => e.Generos)
                .WithMany(g => g.EdiLivros)
                .UsingEntity<Dictionary<string, object>>(
                    "EdiLivroGenero",
                    j => j.HasOne<Genero>().WithMany().HasForeignKey("GeneroId"),
                    j => j.HasOne<EdiLivro>().WithMany().HasForeignKey("EdiLivroId"));

            modelBuilder.Entity<EdiLivro>()
                .HasMany(e => e.Autores)
                .WithMany(a => a.EdiLivros)
                .UsingEntity<Dictionary<string, object>>(
                    "EdiLivroAutor",
                    j => j.HasOne<Autor>().WithMany().HasForeignKey("AutorId"),
                    j => j.HasOne<EdiLivro>().WithMany().HasForeignKey("EdiLivroId"));

            modelBuilder.Entity<EdiLivro>()
                .HasMany(e => e.UniLivros)
                .WithOne(u => u.EdiLivro)
                .HasForeignKey(u => u.Isbn);
        }
    }
}
