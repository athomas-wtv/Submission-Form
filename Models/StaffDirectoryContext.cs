using IST_Submission_Form.Models;
using Microsoft.EntityFrameworkCore;

namespace IST_Submission_Form.Models
{
    public class StaffDirectoryContext : DbContext
    {
        public StaffDirectoryContext(DbContextOptions<StaffDirectoryContext> options) : base(options)
        {
        }
        public DbQuery<Staff> Staff { get; set; }

    }
}
