using Microsoft.EntityFrameworkCore.Migrations;

namespace IST_Submission_Form.Migrations
{
    public partial class StatusField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Submissions",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Submissions");
        }
    }
}
