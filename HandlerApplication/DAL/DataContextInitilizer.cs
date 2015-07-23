﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HandlerApplication.DAL
{
    public class DataContextInitilizer : CreateDatabaseIfNotExists<DataContext>
    {
        protected override void Seed(DataContext context)
        {
            Role role1 = new Role { RoleName = "Admin" };
            Role role2 = new Role { RoleName = "User" };

            //User user1 = new User { Username = "admin", Email = "admin@ymail.com", FirstName = "Admin", Password = "123456", IsActive = true, CreateDate = DateTime.UtcNow, Roles = new List<Role>() };
            //User user2 = new User { Username = "user1", Email = "user1@ymail.com", FirstName = "User1", Password = "123456", IsActive = true, CreateDate = DateTime.UtcNow, Roles = new List<Role>() };
            User user1 = new User { Username = "admin", Email = "admin@ymail.com", Password = "123456", Roles = new List<Role>() };
            User user2 = new User { Username = "user1", Email = "user1@ymail.com", Password = "123456", Roles = new List<Role>() };
            user1.Roles.Add(role1);
            user2.Roles.Add(role2);
            context.Users.Add(user1);
            context.Users.Add(user2);
        }
    }
}