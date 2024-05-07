using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAn.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddActionlog3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateTime",
                table: "ActionLogs",
                newName: "CreatedTime");

            migrationBuilder.RenameColumn(
                name: "CreateBy",
                table: "ActionLogs",
                newName: "UpdatedBy");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "ActionLogs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ActionLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "ActionLogs",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ActionLogs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ActionLogs");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "ActionLogs");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "ActionLogs",
                newName: "CreateBy");

            migrationBuilder.RenameColumn(
                name: "CreatedTime",
                table: "ActionLogs",
                newName: "CreateTime");
        }
    }
}
