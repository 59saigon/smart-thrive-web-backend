using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWD.SmartThrive.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class FixLocationToAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_Course",
                table: "Course");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Location",
                table: "User");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropIndex(
                name: "IX_User_LocationId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Course_LocationId",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "Course",
                newName: "Address");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Course",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Course",
                newName: "LocationId");

            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "User",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Code",
                table: "Course",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    Ward = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_LocationId",
                table: "User",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Course_LocationId",
                table: "Course",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Course",
                table: "Course",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Location",
                table: "User",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id");
        }
    }
}
