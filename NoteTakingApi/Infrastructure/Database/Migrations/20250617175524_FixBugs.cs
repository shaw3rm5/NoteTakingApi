using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoteTakingApi.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class FixBugs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_note_tags_tag_tag_id",
                table: "Note_tags");

            migrationBuilder.DropForeignKey(
                name: "fk_notes_user_user_id",
                table: "Notes");

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "Users",
                column: "email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_note_tags_tags_tag_id",
                table: "Note_tags",
                column: "tag_id",
                principalTable: "Tags",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_notes_users_user_id",
                table: "Notes",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_note_tags_tags_tag_id",
                table: "Note_tags");

            migrationBuilder.DropForeignKey(
                name: "fk_notes_users_user_id",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "ix_users_email",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "fk_note_tags_tag_tag_id",
                table: "Note_tags",
                column: "tag_id",
                principalTable: "Tags",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_notes_user_user_id",
                table: "Notes",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
