﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookswormAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorToBooksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Books",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Books");
        }
    }
}
