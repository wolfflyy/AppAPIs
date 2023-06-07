using System;
using System.Collections.Generic;
using AppAPIs.Repos.Models;
using Microsoft.EntityFrameworkCore;

namespace AppAPIs.Repos;

public partial class apiDBContext : DbContext
{
    public apiDBContext()
    {
    }

    public apiDBContext(DbContextOptions<apiDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Classroom> Classrooms { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Lecturer> Lecturers { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=constring");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Classroom>(entity =>
        {
            entity.HasOne(d => d.Department).WithMany(p => p.Classrooms).HasConstraintName("FK_Classroom_Department");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasOne(d => d.Class).WithMany(p => p.Schedules).HasConstraintName("FK_Schedule_Class");

            entity.HasOne(d => d.Lecturer).WithMany(p => p.Schedules).HasConstraintName("FK_Schedule_Lecturer");

            entity.HasOne(d => d.Room).WithMany(p => p.Schedules).HasConstraintName("FK_Schedule_Classroom");

            entity.HasOne(d => d.Student).WithMany(p => p.Schedules).HasConstraintName("FK_Schedule_Student");

            entity.HasOne(d => d.Sub).WithMany(p => p.Schedules).HasConstraintName("FK_Schedule_Subject");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
