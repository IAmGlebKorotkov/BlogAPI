using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogAPI.Migrations.Community
{
    /// <inheritdoc />
    public partial class CreateCommunityUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "community_users",
                schema: "fias",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    community_id = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_community_users", x => new { x.user_id, x.community_id });
                    table.ForeignKey(
                        name: "FK_community_users_community_community_id",
                        column: x => x.community_id,
                        principalSchema: "fias",
                        principalTable: "community",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_community_users_community_id",
                schema: "fias",
                table: "community_users",
                column: "community_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "community_users",
                schema: "fias");
        }
    }
}
