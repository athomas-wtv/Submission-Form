using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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
        public virtual DbSet<Users> Users { get; set; }

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

                entity.Property(e => e.CommentType)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Commenter)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DateTime).HasColumnType("smalldatetime");

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

                entity.Property(e => e.SubmitDate).HasColumnType("date");

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

                entity.HasOne(p => p.Status).WithMany(s => s.Proposals);
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

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Istmember).HasColumnName("ISTMember");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.NetworkId)
                    .HasColumnName("NetworkID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Telephone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });
        }
    }
}
