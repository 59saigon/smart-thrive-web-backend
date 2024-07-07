using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWD.SmartThrive.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddFỉstLastNameUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoursePackage_Course_CourseId",
                table: "CoursePackage");

            migrationBuilder.DropForeignKey(
                name: "FK_CoursePackage_Package_PackageId",
                table: "CoursePackage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CoursePackage",
                table: "CoursePackage");

            migrationBuilder.RenameTable(
                name: "CoursePackage",
                newName: "CourseXPackage");

            migrationBuilder.RenameIndex(
                name: "IX_CoursePackage_PackageId",
                table: "CourseXPackage",
                newName: "IX_CourseXPackage_PackageId");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Package",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "CourseXPackage",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWId()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseXPackage",
                table: "CourseXPackage",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CourseXPackage_CourseId",
                table: "CourseXPackage",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseXPackage_Course_CourseId",
                table: "CourseXPackage",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseXPackage_Package_PackageId",
                table: "CourseXPackage",
                column: "PackageId",
                principalTable: "Package",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseXPackage_Course_CourseId",
                table: "CourseXPackage");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseXPackage_Package_PackageId",
                table: "CourseXPackage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseXPackage",
                table: "CourseXPackage");

            migrationBuilder.DropIndex(
                name: "IX_CourseXPackage_CourseId",
                table: "CourseXPackage");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "User");

            migrationBuilder.RenameTable(
                name: "CourseXPackage",
                newName: "CoursePackage");

            migrationBuilder.RenameIndex(
                name: "IX_CourseXPackage_PackageId",
                table: "CoursePackage",
                newName: "IX_CoursePackage_PackageId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Package",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "CoursePackage",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "NEWId()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CoursePackage",
                table: "CoursePackage",
                columns: new[] { "CourseId", "PackageId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CoursePackage_Course_CourseId",
                table: "CoursePackage",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CoursePackage_Package_PackageId",
                table: "CoursePackage",
                column: "PackageId",
                principalTable: "Package",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
