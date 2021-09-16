using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace RomEndprojectAwApiRikun.Models
{
    public partial class romdatabaseawContext : DbContext
    {
        public romdatabaseawContext()
        {
        }

        public romdatabaseawContext(DbContextOptions<romdatabaseawContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BrowsingEnd> BrowsingEnds { get; set; }
        public virtual DbSet<BrowsingStart> BrowsingStarts { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<Emotion> Emotions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Finnish_Swedish_CI_AS");

            modelBuilder.Entity<BrowsingEnd>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DeviceId)
                    .HasMaxLength(38)
                    .IsUnicode(false)
                    .HasColumnName("DeviceID");

                entity.Property(e => e.Location)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SessionId)
                    .HasMaxLength(38)
                    .IsUnicode(false)
                    .HasColumnName("SessionID");

                entity.Property(e => e.Time).HasColumnType("datetime");

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.BrowsingEnds)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("endev");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.BrowsingEnds)
                    .HasForeignKey(d => d.SessionId)
                    .HasConstraintName("stend");
            });

            modelBuilder.Entity<BrowsingStart>(entity =>
            {
                entity.HasKey(e => e.SessionId)
                    .HasName("PK__Browsing__C9F492707CF3F87A");

                entity.Property(e => e.SessionId)
                    .HasMaxLength(38)
                    .IsUnicode(false)
                    .HasColumnName("SessionID");

                entity.Property(e => e.DeviceId)
                    .HasMaxLength(38)
                    .IsUnicode(false)
                    .HasColumnName("DeviceID");

                entity.Property(e => e.Domain)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Location)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Time).HasColumnType("datetime");

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.BrowsingStarts)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("stadev");
            });

            modelBuilder.Entity<Device>(entity =>
            {
                entity.Property(e => e.DeviceId)
                    .HasMaxLength(38)
                    .IsUnicode(false)
                    .HasColumnName("DeviceID");

                entity.Property(e => e.LastActive).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Emotion>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DeviceId)
                    .HasMaxLength(38)
                    .IsUnicode(false)
                    .HasColumnName("DeviceID");

                entity.Property(e => e.Time).HasColumnType("datetime");

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.Emotions)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("devemo");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
