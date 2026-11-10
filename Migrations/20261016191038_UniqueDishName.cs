using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Top5.Migrations
{
    /// <inheritdoc />
    public partial class UniqueDishName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Dish_Name",
                table: "Dish",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Dish_Name",
                table: "Dish");
        }
    }
}
