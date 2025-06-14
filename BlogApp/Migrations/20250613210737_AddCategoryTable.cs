using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApp.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Novels");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Novels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Novels_CategoryId",
                table: "Novels",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Novels_Categories_CategoryId",
                table: "Novels",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Novels_Categories_CategoryId",
                table: "Novels");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Novels_CategoryId",
                table: "Novels");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Novels");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Novels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
