using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class OtpRecordUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OtpRecords_AspNetUsers_UserId",
                table: "OtpRecords");

            migrationBuilder.DropIndex(
                name: "IX_OtpRecords_UserId_Type_IsConsumed",
                table: "OtpRecords");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "OtpRecords");

            migrationBuilder.AddColumn<string>(
                name: "Identifier",
                table: "OtpRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Identifier",
                table: "OtpRecords");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "OtpRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OtpRecords_UserId_Type_IsConsumed",
                table: "OtpRecords",
                columns: new[] { "UserId", "Type", "IsConsumed" });

            migrationBuilder.AddForeignKey(
                name: "FK_OtpRecords_AspNetUsers_UserId",
                table: "OtpRecords",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
