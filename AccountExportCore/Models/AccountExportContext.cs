using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AccountExportCore2.Models
{
    public partial class AccountExportContext : DbContext
    {
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<AccountInsurance> AccountInsurance { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Facility> Facility { get; set; }
        public virtual DbSet<Insurance> Insurance { get; set; }
        public virtual DbSet<Patient> Patient { get; set; }

        public AccountExportContext(DbContextOptions<AccountExportContext> options): base(options)
        {
        }

        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //                optionsBuilder.UseSqlServer(@"Server=localhost\sqlexpress;Database=AccountExport;Trusted_Connection=True;User=sa;password=password1");
        //            }
        //        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.AccountNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AdmitDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Balance)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.DischargeDate).HasColumnType("datetime");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_Client");

                entity.HasOne(d => d.Facility)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.FacilityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_Facility");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_Patient");
            });

            modelBuilder.Entity<AccountInsurance>(entity =>
            {
                entity.HasIndex(e => new { e.AccountId, e.InsuranceId })
                    .HasName("IX_AccountInsurance")
                    .IsUnique();

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountInsurance)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountInsurance_Account");

                entity.HasOne(d => d.Insurance)
                    .WithMany(p => p.AccountInsurance)
                    .HasForeignKey(d => d.InsuranceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountInsurance_Insurance");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Facility>(entity =>
            {
                entity.Property(e => e.AddressLine1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AddressLine2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FacilityName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Zip)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Insurance>(entity =>
            {
                entity.Property(e => e.GroupNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PlanName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Policy)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SocialSecuirtyNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
