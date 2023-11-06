using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsSite.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ViewLogsTableFixCancel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ViewLogs_Articles_ArticleId",
                table: "ViewLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ViewLogs",
                table: "ViewLogs");

            migrationBuilder.RenameTable(
                name: "ViewLogs",
                newName: "ViewLog");

            migrationBuilder.RenameIndex(
                name: "IX_ViewLogs_ArticleId",
                table: "ViewLog",
                newName: "IX_ViewLog_ArticleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ViewLog",
                table: "ViewLog",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ViewLog_Articles_ArticleId",
                table: "ViewLog",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ViewLog_Articles_ArticleId",
                table: "ViewLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ViewLog",
                table: "ViewLog");

            migrationBuilder.RenameTable(
                name: "ViewLog",
                newName: "ViewLogs");

            migrationBuilder.RenameIndex(
                name: "IX_ViewLog_ArticleId",
                table: "ViewLogs",
                newName: "IX_ViewLogs_ArticleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ViewLogs",
                table: "ViewLogs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ViewLogs_Articles_ArticleId",
                table: "ViewLogs",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
