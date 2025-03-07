﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using biblioon.Data;

#nullable disable

namespace biblioon.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250103214752_AddEdiLivroBarCode")]
    partial class AddEdiLivroBarCode
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EdiLivroAutor", b =>
                {
                    b.Property<string>("AutorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("EdiLivroId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("AutorId", "EdiLivroId");

                    b.HasIndex("EdiLivroId");

                    b.ToTable("EdiLivroAutor");
                });

            modelBuilder.Entity("EdiLivroGenero", b =>
                {
                    b.Property<string>("EdiLivroId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("GeneroId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("EdiLivroId", "GeneroId");

                    b.HasIndex("GeneroId");

                    b.ToTable("EdiLivroGenero");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("biblioon.Models.Admin", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("DataCriacao")
                        .HasColumnType("datetime2");

                    b.Property<string>("IdCriador")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("IdCriador");

                    b.ToTable("Admins", t =>
                        {
                            t.HasCheckConstraint("CK_Admins_Id_NotEqual_IdCriador", "[Id] <> [IdCriador]");
                        });
                });

            modelBuilder.Entity("biblioon.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Anotacoes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DataAtivacao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Foto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("IsBibliotecario")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("MoradaCodPostal")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MoradaLocalidade")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MoradaRua")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NomeCompleto")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("biblioon.Models.Autor", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Desc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Foto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Autores");
                });

            modelBuilder.Entity("biblioon.Models.Ban", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("DataFim")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DataInicio")
                        .HasColumnType("datetime2");

                    b.Property<string>("IdAdmin")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdUser")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Motivo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdAdmin");

                    b.HasIndex("IdUser");

                    b.ToTable("Bans");
                });

            modelBuilder.Entity("biblioon.Models.Biblioteca", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Horario")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Localização")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Biblioteca");
                });

            modelBuilder.Entity("biblioon.Models.Bibliotecario", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AdminId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DataAtivacao")
                        .HasColumnType("datetime2");

                    b.Property<string>("IdAdminAtivador")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsAtivado")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.HasIndex("IdAdminAtivador");

                    b.ToTable("Bibliotecarios");
                });

            modelBuilder.Entity("biblioon.Models.Comentario", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool?>("Aprovado")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DataCriacao")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("Denunciado")
                        .HasColumnType("bit");

                    b.Property<string>("EdiLivroISBN")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdLeitor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LeitorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("LivroISBN")
                        .HasColumnType("int");

                    b.Property<int?>("Nota")
                        .HasColumnType("int");

                    b.Property<bool?>("Shown")
                        .HasColumnType("bit");

                    b.Property<string>("Texto")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EdiLivroISBN");

                    b.HasIndex("LeitorId");

                    b.ToTable("Comentarios");
                });

            modelBuilder.Entity("biblioon.Models.EdiLivro", b =>
                {
                    b.Property<string>("Isbn")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BarCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Capa")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("DataPublicacao")
                        .HasColumnType("date");

                    b.Property<string>("DescFisica")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EditorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Idioma")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sinopse")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Isbn");

                    b.HasIndex("EditorId");

                    b.ToTable("EdiLivros");
                });

            modelBuilder.Entity("biblioon.Models.Editor", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Descricao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Foto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Site")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Editores");
                });

            modelBuilder.Entity("biblioon.Models.Emprestimo", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("DataEntrega")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DataLevantamento")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataLimiteEntrega")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataRequisitado")
                        .HasColumnType("datetime2");

                    b.Property<string>("EdiLivroISBN")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdBibliotecarioEntrega")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdBibliotecarioLevantamento")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsEntregue")
                        .HasColumnType("bit");

                    b.Property<bool>("IsLevantado")
                        .HasColumnType("bit");

                    b.Property<string>("LeitorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UniLivroId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("EdiLivroISBN");

                    b.HasIndex("IdBibliotecarioEntrega");

                    b.HasIndex("IdBibliotecarioLevantamento");

                    b.HasIndex("LeitorId");

                    b.HasIndex("UniLivroId");

                    b.ToTable("Emprestimos");
                });

            modelBuilder.Entity("biblioon.Models.Genero", b =>
                {
                    b.Property<string>("GeneroId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("GeneroId");

                    b.ToTable("Generos");
                });

            modelBuilder.Entity("biblioon.Models.Leitor", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsBanido")
                        .HasColumnType("bit");

                    b.Property<int>("LivrosLidos")
                        .HasColumnType("int");

                    b.Property<int>("NRequisicoes")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Leitores");
                });

            modelBuilder.Entity("biblioon.Models.Notificacao", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DataLida")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Lida")
                        .HasColumnType("bit");

                    b.Property<string>("Mensagem")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Tipo")
                        .HasColumnType("int");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notificacoes");
                });

            modelBuilder.Entity("biblioon.Models.UniLivro", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Anotacoes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("DataAquisicao")
                        .HasColumnType("date");

                    b.Property<bool>("Disponivel")
                        .HasColumnType("bit");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Isbn")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<float>("PrecoAquisicao")
                        .HasColumnType("real");

                    b.Property<bool>("Requisitado")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("Isbn");

                    b.ToTable("UniLivros");
                });

            modelBuilder.Entity("EdiLivroAutor", b =>
                {
                    b.HasOne("biblioon.Models.Autor", null)
                        .WithMany()
                        .HasForeignKey("AutorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("biblioon.Models.EdiLivro", null)
                        .WithMany()
                        .HasForeignKey("EdiLivroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EdiLivroGenero", b =>
                {
                    b.HasOne("biblioon.Models.EdiLivro", null)
                        .WithMany()
                        .HasForeignKey("EdiLivroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("biblioon.Models.Genero", null)
                        .WithMany()
                        .HasForeignKey("GeneroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("biblioon.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("biblioon.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("biblioon.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("biblioon.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("biblioon.Models.Admin", b =>
                {
                    b.HasOne("biblioon.Models.ApplicationUser", "User")
                        .WithOne()
                        .HasForeignKey("biblioon.Models.Admin", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("biblioon.Models.ApplicationUser", "Criador")
                        .WithMany()
                        .HasForeignKey("IdCriador")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Criador");

                    b.Navigation("User");
                });

            modelBuilder.Entity("biblioon.Models.Ban", b =>
                {
                    b.HasOne("biblioon.Models.Admin", "Admin")
                        .WithMany("BansAplicados")
                        .HasForeignKey("IdAdmin")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("biblioon.Models.Leitor", "User")
                        .WithMany("Bans")
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Admin");

                    b.Navigation("User");
                });

            modelBuilder.Entity("biblioon.Models.Bibliotecario", b =>
                {
                    b.HasOne("biblioon.Models.Admin", null)
                        .WithMany("BibliotecariosAtivados")
                        .HasForeignKey("AdminId");

                    b.HasOne("biblioon.Models.ApplicationUser", "User")
                        .WithOne()
                        .HasForeignKey("biblioon.Models.Bibliotecario", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("biblioon.Models.Admin", "AdminAtivador")
                        .WithMany()
                        .HasForeignKey("IdAdminAtivador")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("AdminAtivador");

                    b.Navigation("User");
                });

            modelBuilder.Entity("biblioon.Models.Comentario", b =>
                {
                    b.HasOne("biblioon.Models.EdiLivro", "EdiLivro")
                        .WithMany("Comentarios")
                        .HasForeignKey("EdiLivroISBN")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("biblioon.Models.Leitor", "Leitor")
                        .WithMany("Comentarios")
                        .HasForeignKey("LeitorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EdiLivro");

                    b.Navigation("Leitor");
                });

            modelBuilder.Entity("biblioon.Models.EdiLivro", b =>
                {
                    b.HasOne("biblioon.Models.Editor", "Editor")
                        .WithMany("EdiLivros")
                        .HasForeignKey("EditorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Editor");
                });

            modelBuilder.Entity("biblioon.Models.Emprestimo", b =>
                {
                    b.HasOne("biblioon.Models.EdiLivro", "EdiLivro")
                        .WithMany("Emprestimos")
                        .HasForeignKey("EdiLivroISBN")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("biblioon.Models.Bibliotecario", "BibliotecarioEntrega")
                        .WithMany("EmprestimosEntrega")
                        .HasForeignKey("IdBibliotecarioEntrega")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("biblioon.Models.Bibliotecario", "BibliotecarioLevantamento")
                        .WithMany("EmprestimosLevantamento")
                        .HasForeignKey("IdBibliotecarioLevantamento")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("biblioon.Models.Leitor", "Leitor")
                        .WithMany("Emprestimos")
                        .HasForeignKey("LeitorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("biblioon.Models.UniLivro", "UniLivro")
                        .WithMany("Emprestimos")
                        .HasForeignKey("UniLivroId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("BibliotecarioEntrega");

                    b.Navigation("BibliotecarioLevantamento");

                    b.Navigation("EdiLivro");

                    b.Navigation("Leitor");

                    b.Navigation("UniLivro");
                });

            modelBuilder.Entity("biblioon.Models.Leitor", b =>
                {
                    b.HasOne("biblioon.Models.ApplicationUser", "User")
                        .WithOne()
                        .HasForeignKey("biblioon.Models.Leitor", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("biblioon.Models.Notificacao", b =>
                {
                    b.HasOne("biblioon.Models.ApplicationUser", "User")
                        .WithMany("Notificacoes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("biblioon.Models.UniLivro", b =>
                {
                    b.HasOne("biblioon.Models.EdiLivro", "EdiLivro")
                        .WithMany("UniLivros")
                        .HasForeignKey("Isbn")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EdiLivro");
                });

            modelBuilder.Entity("biblioon.Models.Admin", b =>
                {
                    b.Navigation("BansAplicados");

                    b.Navigation("BibliotecariosAtivados");
                });

            modelBuilder.Entity("biblioon.Models.ApplicationUser", b =>
                {
                    b.Navigation("Notificacoes");
                });

            modelBuilder.Entity("biblioon.Models.Bibliotecario", b =>
                {
                    b.Navigation("EmprestimosEntrega");

                    b.Navigation("EmprestimosLevantamento");
                });

            modelBuilder.Entity("biblioon.Models.EdiLivro", b =>
                {
                    b.Navigation("Comentarios");

                    b.Navigation("Emprestimos");

                    b.Navigation("UniLivros");
                });

            modelBuilder.Entity("biblioon.Models.Editor", b =>
                {
                    b.Navigation("EdiLivros");
                });

            modelBuilder.Entity("biblioon.Models.Leitor", b =>
                {
                    b.Navigation("Bans");

                    b.Navigation("Comentarios");

                    b.Navigation("Emprestimos");
                });

            modelBuilder.Entity("biblioon.Models.UniLivro", b =>
                {
                    b.Navigation("Emprestimos");
                });
#pragma warning restore 612, 618
        }
    }
}
