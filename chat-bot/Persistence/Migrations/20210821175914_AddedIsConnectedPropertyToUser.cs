using Microsoft.EntityFrameworkCore.Migrations;

namespace Chat.Persistence.Migrations
{
    public partial class AddedIsConnectedPropertyToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsConnected",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsConnected",
                table: "AspNetUsers");
        }
    }
}
