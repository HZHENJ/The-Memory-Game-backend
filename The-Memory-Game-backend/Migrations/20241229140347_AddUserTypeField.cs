using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace The_Memory_Game_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddUserTypeField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "type",
                table: "Users",
                newName: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Users",
                newName: "type");
        }
    }
}
