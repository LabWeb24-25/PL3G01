using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using biblioon.Models;

namespace biblioon.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
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
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Leitor> Leitores { get; set; }
        public DbSet<Ban> Bans { get; set; }
        public DbSet<Bibliotecario> Bibliotecarios { get; set; }
        public DbSet<Emprestimo> Emprestimos { get; set; }
        public DbSet<Notificacao> Notificacoes { get; set; }

        public DbSet<Comentario> Comentarios { get; set; }

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


            modelBuilder.Entity<Leitor>()
                .HasOne(l => l.User)
                .WithOne()
                .HasForeignKey<Leitor>(l => l.Id);

            modelBuilder.Entity<Leitor>()
                .HasMany(l => l.Bans)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.IdUser)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Admin>()
                .HasMany(a => a.BansAplicados)
                .WithOne(b => b.Admin)
                .HasForeignKey(b => b.IdAdmin);

            modelBuilder.Entity<Admin>()
                .HasMany(a => a.BibliotecariosAtivados)
                .WithOne(b => b.AdminAtivador)
                .HasForeignKey(b => b.IdAdminAtivador);

            modelBuilder.Entity<Admin>()
                .HasOne(a => a.User)
                .WithOne()
                .HasForeignKey<Admin>(a => a.Id);

            modelBuilder.Entity<Admin>()
                .HasOne(a => a.Criador)
                .WithMany()
                .HasForeignKey(a => a.IdCriador)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Bibliotecario>()
                .HasOne(b => b.User)
                .WithOne()
                .HasForeignKey<Bibliotecario>(b => b.Id);

            modelBuilder.Entity<Bibliotecario>()
                .HasOne(b => b.AdminAtivador)
                .WithMany()
                .HasForeignKey(b => b.IdAdminAtivador)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Admin>()
                .ToTable(tb => tb.HasCheckConstraint("CK_Admins_Id_NotEqual_IdCriador", "[Id] <> [IdCriador]"));

            modelBuilder.Entity<Emprestimo>()
                .HasOne(e => e.Leitor)
                .WithMany(l => l.Emprestimos)
                .HasForeignKey(e => e.LeitorId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Emprestimo>()
                .HasOne(e => e.UniLivro)
                .WithMany(u => u.Emprestimos)
                .HasForeignKey(e => e.UniLivroId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Emprestimo>()
                .HasOne(e => e.EdiLivro)
                .WithMany(ed => ed.Emprestimos)
                .HasForeignKey(e => e.EdiLivroISBN)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Emprestimo>()
                .HasOne(e => e.BibliotecarioLevantamento)
                .WithMany(b => b.EmprestimosLevantamento)
                .HasForeignKey(e => e.IdBibliotecarioLevantamento)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Emprestimo>()
                .HasOne(e => e.BibliotecarioEntrega)
                .WithMany(b => b.EmprestimosEntrega)
                .HasForeignKey(e => e.IdBibliotecarioEntrega)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Notificacoes)
                .WithOne(n => n.User)
                .HasForeignKey(n => n.UserId);

            modelBuilder.Entity<Notificacao>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notificacoes)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Comentario>()
                .HasOne(c => c.Leitor)
                .WithMany(l => l.Comentarios)
                .HasForeignKey(c => c.LeitorId);

            modelBuilder.Entity<Leitor>()
                .HasMany(u => u.Comentarios)
                .WithOne(c => c.Leitor)
                .HasForeignKey(c => c.LeitorId);

            modelBuilder.Entity<Comentario>()
                .HasOne(c => c.EdiLivro)
                .WithMany(e => e.Comentarios)
                .HasForeignKey(c => c.EdiLivroISBN);

            modelBuilder.Entity<EdiLivro>()
                .HasMany(e => e.Comentarios)
                .WithOne(c => c.EdiLivro)
                .HasForeignKey(c => c.EdiLivroISBN);

            modelBuilder.Entity<Editor>()
                .HasMany(e => e.EdiLivros)
                .WithOne(el => el.Editor)
                .HasForeignKey(el => el.EditorId);

        }
    }
}
