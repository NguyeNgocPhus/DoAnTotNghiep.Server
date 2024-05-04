using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAn.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class isDeleteduser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AppUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AppUsers");
        }
    }
}
