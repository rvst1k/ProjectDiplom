using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ExerciseComplex;

public partial class DiplomContext : DbContext
{
    public DiplomContext()
    {
    }

    public DiplomContext(DbContextOptions<DiplomContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Complex> Complexes { get; set; }

    public virtual DbSet<Exercise> Exercises { get; set; }

    public virtual DbSet<ExerciseAim> ExerciseAims { get; set; }

    public virtual DbSet<ExerciseComplex> ExerciseComplexes { get; set; }

    public virtual DbSet<ExerciseDifficulty> ExerciseDifficulties { get; set; }

    public virtual DbSet<ExerciseLimitation> ExerciseLimitations { get; set; }

    public virtual DbSet<ExerciseType> ExerciseTypes { get; set; }

    public virtual DbSet<Limitation> Limitations { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Diplom;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Cyrillic_General_CI_AS");

        modelBuilder.Entity<Complex>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Complex__3213E83F4454B72C");

            entity.ToTable("Complex");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Complexes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Complex__UserID__5BE2A6F2");
        });

        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Exercise__3213E83F7679522A");

            entity.ToTable("Exercise");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AimId).HasColumnName("AimID");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.DifficultyId).HasColumnName("DifficultyID");
            entity.Property(e => e.Link)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Preview)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TypeId).HasColumnName("TypeID");

            entity.HasOne(d => d.Aim).WithMany(p => p.Exercises)
                .HasForeignKey(d => d.AimId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Exercise__AimID__3E52440B");

            entity.HasOne(d => d.Difficulty).WithMany(p => p.Exercises)
                .HasForeignKey(d => d.DifficultyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Exercise__Diffic__3D5E1FD2");

            entity.HasOne(d => d.Type).WithMany(p => p.Exercises)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Exercise__TypeID__3C69FB99");
        });

        modelBuilder.Entity<ExerciseAim>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Exercise__3213E83F5573DD98");

            entity.ToTable("ExerciseAim");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ExerciseComplex>(entity =>
        {
            entity.HasKey(e => e.ExerciseId).HasName("PK__Exercise__A074AD0F537ABB4D");

            entity.ToTable("ExerciseComplex");

            entity.Property(e => e.ExerciseId)
                .ValueGeneratedNever()
                .HasColumnName("ExerciseID");
            entity.Property(e => e.ComplexId).HasColumnName("ComplexID");

            entity.HasOne(d => d.Complex).WithMany(p => p.ExerciseComplexes)
                .HasForeignKey(d => d.ComplexId)
                .HasConstraintName("FK__ExerciseC__Compl__4222D4EF");

            entity.HasOne(d => d.Exercise).WithOne(p => p.ExerciseComplex)
                .HasForeignKey<ExerciseComplex>(d => d.ExerciseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ExerciseC__Exerc__412EB0B6");
        });

        modelBuilder.Entity<ExerciseDifficulty>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Exercise__3213E83F57A93298");

            entity.ToTable("ExerciseDifficulty");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ExerciseLimitation>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ExerciseLimitation");

            entity.Property(e => e.ExerciseId).HasColumnName("ExerciseID");
            entity.Property(e => e.LimitationId).HasColumnName("LimitationID");

            entity.HasOne(d => d.Exercise).WithMany()
                .HasForeignKey(d => d.ExerciseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ExerciseL__Exerc__5EBF139D");

            entity.HasOne(d => d.Limitation).WithMany()
                .HasForeignKey(d => d.LimitationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ExerciseL__Limit__5FB337D6");
        });

        modelBuilder.Entity<ExerciseType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Exercise__3213E83FDD701E7A");

            entity.ToTable("ExerciseType");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Limitation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Limitati__3213E83F98E3AB31");

            entity.ToTable("Limitation");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3213E83FC6B0017A");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NameOfRole)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3213E83F5FED26F6");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Patronymic)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProfilePicture)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RolesId).HasColumnName("Roles_ID");
            entity.Property(e => e.Surname)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Roles).WithMany(p => p.Users)
                .HasForeignKey(d => d.RolesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__Roles_ID__619B8048");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
