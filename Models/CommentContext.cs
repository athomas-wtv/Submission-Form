using IST_Submission_Form.Models;
using Microsoft.EntityFrameworkCore;

namespace IST_Submission_Form.Models
{
    public class CommentContext : DbContext
    {
        public CommentContext(DbContextOptions<CommentContext> options) : base(options)
        {
        }
        public DbSet<Comment> Comments { get; set; }

    }
}
