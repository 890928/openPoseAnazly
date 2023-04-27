using Microsoft.EntityFrameworkCore.Migrations;

namespace webapi.Migrations
{
    public partial class Add_DeleteOrNo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DeleteOrNo",
                table: "Members",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteOrNo",
                table: "Members");
        }
    }
}
