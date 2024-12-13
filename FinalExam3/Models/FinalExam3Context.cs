using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FinalExam3.Models;

public partial class FinalExam3Context : DbContext
{
    public FinalExam3Context()
    {
    }

    public FinalExam3Context(DbContextOptions<FinalExam3Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Registeration> Registerations { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseNumber).HasName("PK__Courses__A98290EC0F9A63D9");

            entity.Property(e => e.CourseNumber).HasMaxLength(10);
            entity.Property(e => e.CourseTitle).HasMaxLength(100);
        });

        modelBuilder.Entity<Registeration>(entity =>
        {
            entity.ToTable("Registeration");

            entity.Property(e => e.CourseNumber).HasMaxLength(10);

            entity.HasOne(d => d.CourseNumberNavigation).WithMany(p => p.Registerations)
                .HasForeignKey(d => d.CourseNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Registeration_Courses");

            entity.HasOne(d => d.StudentsNumberNavigation).WithMany(p => p.Registerations)
                .HasForeignKey(d => d.StudentsNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Registeration_Students");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentNumber).HasName("PK__Students__DD81BF6DCE1F9083");

            entity.Property(e => e.StudentNumber).ValueGeneratedNever();
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserCode).HasName("PK__Users__1DF52D0DE4206795");

            entity.Property(e => e.UserCode).ValueGeneratedNever();
            entity.Property(e => e.Password).HasMaxLength(50);

            entity.HasOne(d => d.UserCodeNavigation).WithOne(p => p.User)
                .HasForeignKey<User>(d => d.UserCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__UserCode__4D94879B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
