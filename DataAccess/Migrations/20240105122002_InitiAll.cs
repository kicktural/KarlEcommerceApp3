using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitiAll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CartDetail",
                table: "ProductLanguages");

            migrationBuilder.DropColumn(
                name: "Information",
                table: "ProductLanguages");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "ProductLanguages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CartDetail",
                table: "ProductLanguages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Information",
                table: "ProductLanguages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ProductLanguages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
