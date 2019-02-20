using IST_Submission_Form.Models;
using Microsoft.EntityFrameworkCore;

namespace IST_Submission_Form.Models
{
    public class ProposalContext : DbContext
    {
        public ProposalContext(DbContextOptions<ProposalContext> options) : base(options)
        {
        }
        public DbSet<Proposal> Proposals { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Proposal)
                .WithMany(s => s.ISTComments)
                .HasForeignKey(c => c.ProposalID);
        }
    }

}
