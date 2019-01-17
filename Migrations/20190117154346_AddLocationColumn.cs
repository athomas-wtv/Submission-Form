using Microsoft.EntityFrameworkCore.Migrations;

namespace IST_Submission_Form.Migrations
{
    public partial class AddLocationColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Submissions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Submissions");
        }
    }
}
