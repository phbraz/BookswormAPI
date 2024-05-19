using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookswormAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class NewDatastructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_AspNetUsers_UserId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_UserId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "IsFavourite",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Books",
                newName: "contributor");

            migrationBuilder.AddColumn<string>(
                name: "BookImage",
                table: "Books",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UserFavouriteBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    BookId = table.Column<int>(type: "INTEGER", nullable: false),
                    Rate = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavouriteBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFavouriteBooks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFavouriteBooks_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFavouriteBooks_BookId",
                table: "UserFavouriteBooks",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavouriteBooks_UserId",
                table: "UserFavouriteBooks",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFavouriteBooks");

            migrationBuilder.DropColumn(
                name: "BookImage",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "contributor",
                table: "Books",
                newName: "UserId");

            migrationBuilder.AddColumn<bool>(
                name: "IsFavourite",
                table: "Books",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Rate",
                table: "Books",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Books_UserId",
                table: "Books",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_AspNetUsers_UserId",
                table: "Books",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
