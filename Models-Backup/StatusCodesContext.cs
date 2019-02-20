using IST_Submission_Form.Models;
using Microsoft.EntityFrameworkCore;

namespace IST_Submission_Form.Models
{
    public class StatusCodesContext : DbContext
    {
        public StatusCodesContext(DbContextOptions<StatusCodesContext> options) : base(options)
        {
        }
        public DbQuery<StatusCodes> StatusCode { get; set; }

    }
}
