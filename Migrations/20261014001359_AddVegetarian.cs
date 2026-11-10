using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Top5.Migrations
{
    /// <inheritdoc />
    public partial class AddVegetarian : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVegetarian",
                table: "Dish",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Dish",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsVegetarian",
                value: false);

            migrationBuilder.UpdateData(
                table: "Dish",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsVegetarian",
                value: false);

            migrationBuilder.UpdateData(
                table: "Dish",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsVegetarian",
                value: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVegetarian",
                table: "Dishes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVegetarian",
                table: "Dish");
        }
    }
}
