using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password", "PasswordSalt" },
                values: new object[,]
                {
                    { new Guid("41580610-42fb-48de-8825-08b311021f4b"), "bob@example.com", "Bob", new byte[0], new byte[0] },
                    { new Guid("8e1be324-bee0-446f-902d-498762b9483f"), "alice@example.com", "Alice", new byte[0], new byte[0] }
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Categories", "Deadline", "Description", "OwnerId", "Priority", "Status", "Tags", "Title" },
                values: new object[,]
                {
                    { new Guid("5704f4c0-9572-498c-924f-6606cc4177a2"), null, new DateTime(2024, 5, 26, 18, 50, 56, 964, DateTimeKind.Utc).AddTicks(1769), "Description for Task 1", new Guid("8e1be324-bee0-446f-902d-498762b9483f"), "High", null, null, "Task 1" },
                    { new Guid("9eb35740-cba3-45d6-a8e8-e401a9955831"), null, new DateTime(2024, 6, 2, 18, 50, 56, 964, DateTimeKind.Utc).AddTicks(1777), "Description for Task 2", new Guid("41580610-42fb-48de-8825-08b311021f4b"), "Medium", null, null, "Task 2" }
                });

            migrationBuilder.InsertData(
                table: "UserTasks",
                columns: new[] { "TaskId", "UserId" },
                values: new object[,]
                {
                    { new Guid("5704f4c0-9572-498c-924f-6606cc4177a2"), new Guid("41580610-42fb-48de-8825-08b311021f4b") },
                    { new Guid("5704f4c0-9572-498c-924f-6606cc4177a2"), new Guid("8e1be324-bee0-446f-902d-498762b9483f") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("9eb35740-cba3-45d6-a8e8-e401a9955831"));

            migrationBuilder.DeleteData(
                table: "UserTasks",
                keyColumns: new[] { "TaskId", "UserId" },
                keyValues: new object[] { new Guid("5704f4c0-9572-498c-924f-6606cc4177a2"), new Guid("41580610-42fb-48de-8825-08b311021f4b") });

            migrationBuilder.DeleteData(
                table: "UserTasks",
                keyColumns: new[] { "TaskId", "UserId" },
                keyValues: new object[] { new Guid("5704f4c0-9572-498c-924f-6606cc4177a2"), new Guid("8e1be324-bee0-446f-902d-498762b9483f") });

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("5704f4c0-9572-498c-924f-6606cc4177a2"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("41580610-42fb-48de-8825-08b311021f4b"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8e1be324-bee0-446f-902d-498762b9483f"));
        }
    }
}
