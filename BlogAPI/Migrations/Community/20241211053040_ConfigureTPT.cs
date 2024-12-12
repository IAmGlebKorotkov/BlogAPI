using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogAPI.Migrations.Community
{
    /// <inheritdoc />
    public partial class ConfigureTPT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_community_users_community_community_id",
                schema: "fias",
                table: "community_users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_community_users",
                schema: "fias",
                table: "community_users");

            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.RenameTable(
                name: "community_users",
                schema: "fias",
                newName: "user_community",
                newSchema: "fias");

            migrationBuilder.RenameIndex(
                name: "IX_community_users_community_id",
                schema: "fias",
                table: "user_community",
                newName: "IX_user_community_community_id");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                schema: "fias",
                table: "community",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "role",
                schema: "fias",
                table: "user_community",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_community",
                schema: "fias",
                table: "user_community",
                columns: new[] { "user_id", "community_id" });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "text", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "text", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "community_full",
                schema: "fias",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_community_full", x => x.id);
                    table.ForeignKey(
                        name: "FK_community_full_community_id",
                        column: x => x.id,
                        principalSchema: "fias",
                        principalTable: "community",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProfileDto",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    CommunityFullDtoId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfileDto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfileDto_community_full_CommunityFullDtoId",
                        column: x => x.CommunityFullDtoId,
                        principalSchema: "fias",
                        principalTable: "community_full",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileDto_CommunityFullDtoId",
                table: "UserProfileDto",
                column: "CommunityFullDtoId");

            migrationBuilder.AddForeignKey(
                name: "FK_user_community_AspNetUsers_user_id",
                schema: "fias",
                table: "user_community",
                column: "user_id",
                principalSchema: "public",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_community_community_community_id",
                schema: "fias",
                table: "user_community",
                column: "community_id",
                principalSchema: "fias",
                principalTable: "community",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_community_AspNetUsers_user_id",
                schema: "fias",
                table: "user_community");

            migrationBuilder.DropForeignKey(
                name: "FK_user_community_community_community_id",
                schema: "fias",
                table: "user_community");

            migrationBuilder.DropTable(
                name: "AspNetUsers",
                schema: "public");

            migrationBuilder.DropTable(
                name: "UserProfileDto");

            migrationBuilder.DropTable(
                name: "community_full",
                schema: "fias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_community",
                schema: "fias",
                table: "user_community");

            migrationBuilder.RenameTable(
                name: "user_community",
                schema: "fias",
                newName: "community_users",
                newSchema: "fias");

            migrationBuilder.RenameIndex(
                name: "IX_user_community_community_id",
                schema: "fias",
                table: "community_users",
                newName: "IX_community_users_community_id");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                schema: "fias",
                table: "community",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "role",
                schema: "fias",
                table: "community_users",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_community_users",
                schema: "fias",
                table: "community_users",
                columns: new[] { "user_id", "community_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_community_users_community_community_id",
                schema: "fias",
                table: "community_users",
                column: "community_id",
                principalSchema: "fias",
                principalTable: "community",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
