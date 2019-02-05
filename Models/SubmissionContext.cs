using IST_Submission_Form.Models;
using Microsoft.EntityFrameworkCore;

namespace IST_Submission_Form.Models
{
    public class SubmissionContext : DbContext
    {
        public SubmissionContext(DbContextOptions<SubmissionContext> options) : base(options)
        {
        }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Submission)
                .WithMany(s => s.Comments)
                .HasForeignKey(c => c.SubmissionID);
        }
    }

}
