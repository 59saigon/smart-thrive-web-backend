using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWD.SmartThrive.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class FixCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalSlot",
                table: "Course",
                newName: "TotalSlots");

            migrationBuilder.RenameColumn(
                name: "Sold_product",
                table: "Course",
                newName: "TotalSessions");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Course",
                newName: "SoldCourses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalSlots",
                table: "Course",
                newName: "TotalSlot");

            migrationBuilder.RenameColumn(
                name: "TotalSessions",
                table: "Course",
                newName: "Sold_product");

            migrationBuilder.RenameColumn(
                name: "SoldCourses",
                table: "Course",
                newName: "Quantity");
        }
    }
}
