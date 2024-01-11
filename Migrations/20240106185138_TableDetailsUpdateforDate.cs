using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodePulse.Migrations
{
    /// <inheritdoc />
    public partial class TableDetailsUpdateforDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "BlogPosts",
                newName: "publishedDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "publishedDate",
                table: "BlogPosts",
                newName: "DateCreated");
        }
    }
}
