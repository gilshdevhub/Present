using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Identity.Migrations
{
    public partial class PagesRolesNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "title",
                table: "Page",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "frontPath",
                table: "Page",
                newName: "FrontPath");

            migrationBuilder.RenameColumn(
                name: "folder",
                table: "Page",
                newName: "Folder");

            migrationBuilder.RenameColumn(
                name: "editComponent",
                table: "Page",
                newName: "EditComponent");

            migrationBuilder.RenameColumn(
                name: "controller",
                table: "Page",
                newName: "Controller");

            migrationBuilder.RenameColumn(
                name: "component",
                table: "Page",
                newName: "Component");

            migrationBuilder.RenameColumn(
                name: "classProp",
                table: "Page",
                newName: "ClassProp");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PageRoleNew",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    PageId = table.Column<int>(type: "int", nullable: false),
                    Readable = table.Column<bool>(type: "bit", nullable: false),
                    Updatable = table.Column<bool>(type: "bit", nullable: false),
                    Visible = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageRoleNew", x => new { x.RoleId, x.PageId });
                    table.ForeignKey(
                        name: "FK_PageRoleNew_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PageRoleNew_Page_PageId",
                        column: x => x.PageId,
                        principalTable: "Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PageRoleNew_PageId",
                table: "PageRoleNew",
                column: "PageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PageRoleNew");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Page",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "FrontPath",
                table: "Page",
                newName: "frontPath");

            migrationBuilder.RenameColumn(
                name: "Folder",
                table: "Page",
                newName: "folder");

            migrationBuilder.RenameColumn(
                name: "EditComponent",
                table: "Page",
                newName: "editComponent");

            migrationBuilder.RenameColumn(
                name: "Controller",
                table: "Page",
                newName: "controller");

            migrationBuilder.RenameColumn(
                name: "Component",
                table: "Page",
                newName: "component");

            migrationBuilder.RenameColumn(
                name: "ClassProp",
                table: "Page",
                newName: "classProp");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
