using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace biblioon.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaTudo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Anotacoes",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataAtivacao",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCriacao",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Foto",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBibliotecario",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MoradaCodPostal",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MoradaLocalidade",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MoradaRua",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomeCompleto",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdCriador = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                    table.CheckConstraint("CK_Admins_Id_NotEqual_IdCriador", "[Id] <> [IdCriador]");
                    table.ForeignKey(
                        name: "FK_Admins_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Admins_AspNetUsers_IdCriador",
                        column: x => x.IdCriador,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Autores",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Biblioteca",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Localização = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Horario = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biblioteca", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Editores",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Site = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Editores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Generos",
                columns: table => new
                {
                    GeneroId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Generos", x => x.GeneroId);
                });

            migrationBuilder.CreateTable(
                name: "Leitores",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LivrosLidos = table.Column<int>(type: "int", nullable: false),
                    NRequisicoes = table.Column<int>(type: "int", nullable: false),
                    IsBanido = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leitores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Leitores_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notificacoes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mensagem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Lida = table.Column<bool>(type: "bit", nullable: false),
                    DataLida = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notificacoes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Bibliotecarios",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdAdminAtivador = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DataAtivacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAtivado = table.Column<bool>(type: "bit", nullable: false),
                    AdminId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bibliotecarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bibliotecarios_Admins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admins",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bibliotecarios_Admins_IdAdminAtivador",
                        column: x => x.IdAdminAtivador,
                        principalTable: "Admins",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bibliotecarios_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EdiLivros",
                columns: table => new
                {
                    Isbn = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sinopse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Idioma = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescFisica = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataPublicacao = table.Column<DateOnly>(type: "date", nullable: false),
                    EditorId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EdiLivros", x => x.Isbn);
                    table.ForeignKey(
                        name: "FK_EdiLivros_Editores_EditorId",
                        column: x => x.EditorId,
                        principalTable: "Editores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bans",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdUser = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdAdmin = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Motivo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bans_Admins_IdAdmin",
                        column: x => x.IdAdmin,
                        principalTable: "Admins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bans_Leitores_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Leitores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Comentarios",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EdiLivroISBN = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LeitorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Texto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Nota = table.Column<int>(type: "int", nullable: true),
                    Aprovado = table.Column<bool>(type: "bit", nullable: true),
                    Denunciado = table.Column<bool>(type: "bit", nullable: true),
                    Shown = table.Column<bool>(type: "bit", nullable: true),
                    IdLeitor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LivroISBN = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comentarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comentarios_EdiLivros_EdiLivroISBN",
                        column: x => x.EdiLivroISBN,
                        principalTable: "EdiLivros",
                        principalColumn: "Isbn",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comentarios_Leitores_LeitorId",
                        column: x => x.LeitorId,
                        principalTable: "Leitores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EdiLivroAutor",
                columns: table => new
                {
                    AutorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EdiLivroId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EdiLivroAutor", x => new { x.AutorId, x.EdiLivroId });
                    table.ForeignKey(
                        name: "FK_EdiLivroAutor_Autores_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Autores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EdiLivroAutor_EdiLivros_EdiLivroId",
                        column: x => x.EdiLivroId,
                        principalTable: "EdiLivros",
                        principalColumn: "Isbn",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EdiLivroGenero",
                columns: table => new
                {
                    EdiLivroId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GeneroId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EdiLivroGenero", x => new { x.EdiLivroId, x.GeneroId });
                    table.ForeignKey(
                        name: "FK_EdiLivroGenero_EdiLivros_EdiLivroId",
                        column: x => x.EdiLivroId,
                        principalTable: "EdiLivros",
                        principalColumn: "Isbn",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EdiLivroGenero_Generos_GeneroId",
                        column: x => x.GeneroId,
                        principalTable: "Generos",
                        principalColumn: "GeneroId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UniLivros",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Isbn = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrecoAquisicao = table.Column<float>(type: "real", nullable: false),
                    DataAquisicao = table.Column<DateOnly>(type: "date", nullable: false),
                    Requisitado = table.Column<bool>(type: "bit", nullable: false),
                    Disponivel = table.Column<bool>(type: "bit", nullable: false),
                    Anotacoes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniLivros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UniLivros_EdiLivros_Isbn",
                        column: x => x.Isbn,
                        principalTable: "EdiLivros",
                        principalColumn: "Isbn",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Emprestimos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LeitorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UniLivroId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EdiLivroISBN = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DataRequisitado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataLimiteEntrega = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataLevantamento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataEntrega = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsLevantado = table.Column<bool>(type: "bit", nullable: false),
                    IsEntregue = table.Column<bool>(type: "bit", nullable: false),
                    IdBibliotecarioLevantamento = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdBibliotecarioEntrega = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emprestimos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Emprestimos_Bibliotecarios_IdBibliotecarioEntrega",
                        column: x => x.IdBibliotecarioEntrega,
                        principalTable: "Bibliotecarios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Emprestimos_Bibliotecarios_IdBibliotecarioLevantamento",
                        column: x => x.IdBibliotecarioLevantamento,
                        principalTable: "Bibliotecarios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Emprestimos_EdiLivros_EdiLivroISBN",
                        column: x => x.EdiLivroISBN,
                        principalTable: "EdiLivros",
                        principalColumn: "Isbn");
                    table.ForeignKey(
                        name: "FK_Emprestimos_Leitores_LeitorId",
                        column: x => x.LeitorId,
                        principalTable: "Leitores",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Emprestimos_UniLivros_UniLivroId",
                        column: x => x.UniLivroId,
                        principalTable: "UniLivros",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admins_IdCriador",
                table: "Admins",
                column: "IdCriador");

            migrationBuilder.CreateIndex(
                name: "IX_Bans_IdAdmin",
                table: "Bans",
                column: "IdAdmin");

            migrationBuilder.CreateIndex(
                name: "IX_Bans_IdUser",
                table: "Bans",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Bibliotecarios_AdminId",
                table: "Bibliotecarios",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Bibliotecarios_IdAdminAtivador",
                table: "Bibliotecarios",
                column: "IdAdminAtivador");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_EdiLivroISBN",
                table: "Comentarios",
                column: "EdiLivroISBN");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_LeitorId",
                table: "Comentarios",
                column: "LeitorId");

            migrationBuilder.CreateIndex(
                name: "IX_EdiLivroAutor_EdiLivroId",
                table: "EdiLivroAutor",
                column: "EdiLivroId");

            migrationBuilder.CreateIndex(
                name: "IX_EdiLivroGenero_GeneroId",
                table: "EdiLivroGenero",
                column: "GeneroId");

            migrationBuilder.CreateIndex(
                name: "IX_EdiLivros_EditorId",
                table: "EdiLivros",
                column: "EditorId");

            migrationBuilder.CreateIndex(
                name: "IX_Emprestimos_EdiLivroISBN",
                table: "Emprestimos",
                column: "EdiLivroISBN");

            migrationBuilder.CreateIndex(
                name: "IX_Emprestimos_IdBibliotecarioEntrega",
                table: "Emprestimos",
                column: "IdBibliotecarioEntrega");

            migrationBuilder.CreateIndex(
                name: "IX_Emprestimos_IdBibliotecarioLevantamento",
                table: "Emprestimos",
                column: "IdBibliotecarioLevantamento");

            migrationBuilder.CreateIndex(
                name: "IX_Emprestimos_LeitorId",
                table: "Emprestimos",
                column: "LeitorId");

            migrationBuilder.CreateIndex(
                name: "IX_Emprestimos_UniLivroId",
                table: "Emprestimos",
                column: "UniLivroId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_UserId",
                table: "Notificacoes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UniLivros_Isbn",
                table: "UniLivros",
                column: "Isbn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bans");

            migrationBuilder.DropTable(
                name: "Biblioteca");

            migrationBuilder.DropTable(
                name: "Comentarios");

            migrationBuilder.DropTable(
                name: "EdiLivroAutor");

            migrationBuilder.DropTable(
                name: "EdiLivroGenero");

            migrationBuilder.DropTable(
                name: "Emprestimos");

            migrationBuilder.DropTable(
                name: "Notificacoes");

            migrationBuilder.DropTable(
                name: "Autores");

            migrationBuilder.DropTable(
                name: "Generos");

            migrationBuilder.DropTable(
                name: "Bibliotecarios");

            migrationBuilder.DropTable(
                name: "Leitores");

            migrationBuilder.DropTable(
                name: "UniLivros");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "EdiLivros");

            migrationBuilder.DropTable(
                name: "Editores");

            migrationBuilder.DropColumn(
                name: "Anotacoes",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DataAtivacao",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DataCriacao",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Foto",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsBibliotecario",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MoradaCodPostal",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MoradaLocalidade",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MoradaRua",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NomeCompleto",
                table: "AspNetUsers");
        }
    }
}
