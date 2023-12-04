using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace APIToDoList.Model;

public partial class DbdebianContext : DbContext
{
    public DbdebianContext()
    {
    }

    public DbdebianContext(DbContextOptions<DbdebianContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TaskT> TaskTs { get; set; }

    public virtual DbSet<UserT> UserTs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=dbdebian;UserName=dbuser;Password=chloe700A");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskT>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("task_t_pkey");

            entity.ToTable("task_t", "databasedebian");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(45);

            entity.HasOne(d => d.User).WithMany(p => p.TaskTs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserId_FK");
        });

        modelBuilder.Entity<UserT>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_t_pkey");

            entity.ToTable("user_t", "databasedebian");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(45);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
