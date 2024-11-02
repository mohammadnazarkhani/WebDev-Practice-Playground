using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlogWebApp.Api.Data.migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Password", "Role", "UserName" },
                values: new object[,]
                {
                    { 1, "admin@example.com", "hashed_password_here", 2, "admin" },
                    { 2, "creator@example.com", "hashed_password_here", 1, "creator" },
                    { 3, "user@example.com", "hashed_password_here", 0, "user" }
                });

            migrationBuilder.InsertData(
                table: "BlogPosts",
                columns: new[] { "Id", "AuthorId", "Content", "CreatedAt", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, "This is the content of the first blog post.", new DateTime(2024, 11, 2, 22, 26, 45, 419, DateTimeKind.Utc).AddTicks(2246), "First Blog Post", new DateTime(2024, 11, 2, 22, 26, 45, 419, DateTimeKind.Utc).AddTicks(2248) },
                    { 2, 2, "This is the content of the second blog post.", new DateTime(2024, 11, 2, 22, 26, 45, 419, DateTimeKind.Utc).AddTicks(2251), "Second Blog Post", new DateTime(2024, 11, 2, 22, 26, 45, 419, DateTimeKind.Utc).AddTicks(2251) }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "BlogPostId", "CreatedAt", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 11, 2, 22, 26, 45, 419, DateTimeKind.Utc).AddTicks(2266), 3 },
                    { 2, 2, new DateTime(2024, 11, 2, 22, 26, 45, 419, DateTimeKind.Utc).AddTicks(2268), 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
