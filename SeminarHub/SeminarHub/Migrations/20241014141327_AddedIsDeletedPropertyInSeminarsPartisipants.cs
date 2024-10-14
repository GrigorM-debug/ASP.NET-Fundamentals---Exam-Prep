using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeminarHub.Migrations
{
    public partial class AddedIsDeletedPropertyInSeminarsPartisipants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SeminarParticipant",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SeminarParticipant");
        }
    }
}
