using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

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

        public virtual DbSet<CommentsTicket> CommentsTicket { get; set; }
        public virtual DbSet<Proposals> Proposals { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public IConfiguration Configuration { get; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration["ConnectionStrings:ISTProjectsContext"]);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommentsTicket>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.DateTime).HasColumnType("smalldatetime");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.InverseIdNavigation)
                    .HasForeignKey<CommentsTicket>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommentsTicket_CommentsTicket");
            });

            modelBuilder.Entity<Proposals>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

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
