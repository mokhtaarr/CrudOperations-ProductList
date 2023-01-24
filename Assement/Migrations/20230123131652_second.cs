using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assement.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "GenreId",
                table: "products",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<int>(
                name: "GenreId1",
                table: "products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_products_GenreId1",
                table: "products",
                column: "GenreId1");

            migrationBuilder.AddForeignKey(
                name: "FK_products_Genres_GenreId1",
                table: "products",
                column: "GenreId1",
                principalTable: "Genres",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_Genres_GenreId1",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_products_GenreId1",
                table: "products");

            migrationBuilder.DropColumn(
                name: "GenreId",
                table: "products");

            migrationBuilder.DropColumn(
                name: "GenreId1",
                table: "products");
        }
    }
}
