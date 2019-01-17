using Microsoft.EntityFrameworkCore.Migrations;

namespace IST_Submission_Form.Migrations
{
    public partial class AddedLoginID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LoginID",
                table: "Submissions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoginID",
                table: "Submissions");
        }
    }
}
