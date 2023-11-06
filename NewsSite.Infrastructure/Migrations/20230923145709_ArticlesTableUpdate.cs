using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NewsSite.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ArticlesTableUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ArticleBody",
                table: "Articles",
                newName: "PreviewText");

            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "Author", "Body", "DatePublished", "PreviewText", "Title" },
                values: new object[,]
                {
                    { new Guid("021a6a5e-f59b-4d64-8bf5-9461faffabd2"), "Author 4", "This is the body of article 4", new DateTime(2023, 9, 23, 16, 57, 9, 12, DateTimeKind.Local).AddTicks(7766), "This is the preview text of article 4", "Article 4" },
                    { new Guid("0667b7d1-e4c1-4872-abb0-03a2a0301950"), "Author 5", "This is the body of article 5", new DateTime(2023, 9, 23, 16, 57, 9, 12, DateTimeKind.Local).AddTicks(7793), "This is the preview text of article 5", "Article 5" },
                    { new Guid("12c1ff9c-c582-45b0-83b8-5c9d3faf019c"), "Author 1", "This is the body of article 1", new DateTime(2023, 9, 23, 16, 57, 9, 12, DateTimeKind.Local).AddTicks(7609), "This is the preview text of article 1", "Article 1" },
                    { new Guid("789808fd-c124-4d83-897b-1b9823339574"), "Author 7", "This is the body of article 7", new DateTime(2023, 9, 23, 16, 57, 9, 12, DateTimeKind.Local).AddTicks(7802), "This is the preview text of article 7", "Article 7" },
                    { new Guid("9c01c167-b5c4-42fb-97f0-af34fc6f6e7f"), "Author 2", "This is the body of article 2", new DateTime(2023, 9, 23, 16, 57, 9, 12, DateTimeKind.Local).AddTicks(7683), "This is the preview text of article 2", "Article 2" },
                    { new Guid("a9fd0c91-dd88-4187-8cd6-9f2f20b2a050"), "Author 6", "This is the body of article 6", new DateTime(2023, 9, 23, 16, 57, 9, 12, DateTimeKind.Local).AddTicks(7798), "This is the preview text of article 6", "Article 6" },
                    { new Guid("cf1664f0-16e6-4c8c-8eaf-699e3160f9ef"), "Author 3", "This is the body of article 3", new DateTime(2023, 9, 23, 16, 57, 9, 12, DateTimeKind.Local).AddTicks(7688), "This is the preview text of article 3", "Article 3" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: new Guid("021a6a5e-f59b-4d64-8bf5-9461faffabd2"));

            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: new Guid("0667b7d1-e4c1-4872-abb0-03a2a0301950"));

            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: new Guid("12c1ff9c-c582-45b0-83b8-5c9d3faf019c"));

            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: new Guid("789808fd-c124-4d83-897b-1b9823339574"));

            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: new Guid("9c01c167-b5c4-42fb-97f0-af34fc6f6e7f"));

            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: new Guid("a9fd0c91-dd88-4187-8cd6-9f2f20b2a050"));

            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: new Guid("cf1664f0-16e6-4c8c-8eaf-699e3160f9ef"));

            migrationBuilder.DropColumn(
                name: "Body",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "PreviewText",
                table: "Articles",
                newName: "ArticleBody");
        }
    }
}
