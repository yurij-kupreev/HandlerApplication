﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HandlerApplication.DAL
{
    public class DataContext : DbContext
    {
        public DataContext()
            : base("UserDatabase")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .Map(m =>
            {
                m.ToTable("UserRoles");
                m.MapLeftKey("UserId");
                m.MapRightKey("RoleId");
            });
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        
    }
}