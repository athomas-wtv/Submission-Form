using Microsoft.EntityFrameworkCore;

namespace IST_Submission_Form.Models
{
    public partial class ISTProjectsContext : DbContext
    {
        public ISTProjectsContext()
        {
        }

        public ISTProjectsContext(DbContextOptions<ISTProjectsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<Proposals> Proposals { get; set; }
        public virtual DbSet<Status> Status { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=cen-web-sql1,18533;Database=ISTProjects;User ID=webdatawriter;Password=h2@34XtPYx887;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comments>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Commenter)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DateTime)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ProposalId).HasColumnName("ProposalID");
            });

            modelBuilder.Entity<Proposals>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AssignedTo)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.DesiredCompletion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Istcomments)
                    .HasColumnName("ISTComments")
                    .IsUnicode(false);

                entity.Property(e => e.SubmitDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SubmittedBy)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.SubmitterComments).IsUnicode(false);

                entity.Property(e => e.SubmitterEmail)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.SubmitterLocation)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.SubmitterName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.StatusDescription)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });
        }
    }
}
