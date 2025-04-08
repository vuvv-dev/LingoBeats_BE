using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Base.DataBaseAndIdentity.Migrations.M_AppDbContext
{
    /// <inheritdoc />
    public partial class M2_Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                schema: "myspace",
                table: "user_token",
                type: "character varying(34)",
                maxLength: 34,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "expire_at",
                schema: "myspace",
                table: "user_token",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                schema: "myspace",
                table: "user_token");

            migrationBuilder.DropColumn(
                name: "expire_at",
                schema: "myspace",
                table: "user_token");
        }
    }
}
