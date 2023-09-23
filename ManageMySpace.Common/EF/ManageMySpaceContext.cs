using ManageMySpace.Common.EF.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManageMySpace.Common.EF
{
    public class ManageMySpaceContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }


        public ManageMySpaceContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(t => new { t.UserId, t.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(sc => sc.User)
                .WithMany(s => s.UserRoles)
                .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(sc => sc.Role)
                .WithMany(c => c.UserRoles)
                .HasForeignKey(sc => sc.RoleId);

            modelBuilder.Entity<UserActivityVisitor>()
                .HasKey(t => new { t.UserId, t.ActivityId });

            modelBuilder.Entity<UserActivityVisitor>()
                .HasOne(sc => sc.User)
                .WithMany(s => s.VisitedEvents)
                .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<UserActivityVisitor>()
                .HasOne(sc => sc.Activity)
                .WithMany(c => c.Visitors)
                .HasForeignKey(sc => sc.ActivityId);

            modelBuilder.Entity<UserActivityOrganizator>()
                .HasKey(t => new { t.UserId, t.ActivityId });

            modelBuilder.Entity<UserActivityOrganizator>()
                .HasOne(sc => sc.User)
                .WithMany(s => s.OrganizedEvents)
                .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<UserActivityOrganizator>()
                .HasOne(sc => sc.Activity)
                .WithMany(c => c.Organizators)
                .HasForeignKey(sc => sc.ActivityId);

            InitDataBase(modelBuilder);
        }

        private void InitDataBase(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role[]
                {
                    new Role { Id = Guid.NewGuid(), Name = "Admin"},
                    new Role { Id = Guid.NewGuid(), Name = "Student"},
                    new Role { Id = Guid.NewGuid(), Name = "Visitor"}
                });
        }
    }
}
