using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class area2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ActionId",
                table: "Reactions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_ActionId",
                table: "Reactions",
                column: "ActionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_Actions_ActionId",
                table: "Reactions",
                column: "ActionId",
                principalTable: "Actions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_Actions_ActionId",
                table: "Reactions");

            migrationBuilder.DropIndex(
                name: "IX_Reactions_ActionId",
                table: "Reactions");

            migrationBuilder.DropColumn(
                name: "ActionId",
                table: "Reactions");
        }
    }
}
