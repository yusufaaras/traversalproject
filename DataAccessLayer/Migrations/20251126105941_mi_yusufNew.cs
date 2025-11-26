using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mi_yusufNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TestiMonials",
                table: "TestiMonials");

            migrationBuilder.DropPrimaryKey(
                name: "PK_contactUses",
                table: "contactUses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Features2",
                table: "Features2");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Abouts2",
                table: "Abouts2");

            migrationBuilder.RenameTable(
                name: "TestiMonials",
                newName: "Testimonials");

            migrationBuilder.RenameTable(
                name: "contactUses",
                newName: "ContactUses");

            migrationBuilder.RenameTable(
                name: "Features2",
                newName: "Feature2s");

            migrationBuilder.RenameTable(
                name: "Abouts2",
                newName: "About2s");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Testimonials",
                table: "Testimonials",
                column: "TestiMonialId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactUses",
                table: "ContactUses",
                column: "ContantUsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Feature2s",
                table: "Feature2s",
                column: "Feature2Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_About2s",
                table: "About2s",
                column: "About2Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Testimonials",
                table: "Testimonials");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactUses",
                table: "ContactUses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Feature2s",
                table: "Feature2s");

            migrationBuilder.DropPrimaryKey(
                name: "PK_About2s",
                table: "About2s");

            migrationBuilder.RenameTable(
                name: "Testimonials",
                newName: "TestiMonials");

            migrationBuilder.RenameTable(
                name: "ContactUses",
                newName: "contactUses");

            migrationBuilder.RenameTable(
                name: "Feature2s",
                newName: "Features2");

            migrationBuilder.RenameTable(
                name: "About2s",
                newName: "Abouts2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestiMonials",
                table: "TestiMonials",
                column: "TestiMonialId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_contactUses",
                table: "contactUses",
                column: "ContantUsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Features2",
                table: "Features2",
                column: "Feature2Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Abouts2",
                table: "Abouts2",
                column: "About2Id");
        }
    }
}
