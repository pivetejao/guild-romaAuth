using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace guild_romaAuth.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAtToGuildEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "GuildEvents");

            migrationBuilder.RenameColumn(
                name: "EventTime",
                table: "GuildEvents",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<bool>(
                name: "IsMandatory",
                table: "GuildEvents",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "GuildEvents",
                newName: "EventTime");

            migrationBuilder.AlterColumn<string>(
                name: "IsMandatory",
                table: "GuildEvents",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "GuildEvents",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
