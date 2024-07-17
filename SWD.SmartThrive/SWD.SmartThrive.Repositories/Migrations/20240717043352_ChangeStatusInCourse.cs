using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWD.SmartThrive.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class ChangeStatusInCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Course");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Course",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Course");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Course",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Course",
                type: "bit",
                nullable: true);
        }
    }
}
