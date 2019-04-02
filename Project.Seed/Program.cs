﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Project.DAL;
using Project.DAL.Entity;
using Project.DAL.Enums;

namespace Project.Seed
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var dbContext = CreateDbContext())
            {
                ClearDatabase(dbContext);
                SeedData(dbContext);
            }
        }

        private static void SeedData(ProjectDbContext dbContext)
        {
            var user1 = new User
            {
                Name = "Albert",
                Email = "dartwader128@azet.sk",
                Password = "pumkli28",
                Nick = "Albi48"
            };
            dbContext.Users.Add(user1);

            var user2 = new User
            {
                Name = "Daniel",
                Email = "optimus13@pokec.sk",
                Password = "danulko13",
                Nick = "Wiesko"
            };
            dbContext.Users.Add(user2);

            var team = new Team
            {
                Name = "Sicaci",
                Admin = user1.Id,
                Created = DateTime.Today,
                UsersInTeams = 
                {
                    new UsersInTeam()
                    {
                        User = user1
                    }
                }
            };
            dbContext.Teams.Add(team);
            team.UsersInTeams.Add(new UsersInTeam(){User = user2});
            dbContext.Teams.Add(team);

            var post = new Post
            {
                Author = user1,
                Text = "Pekna praca!",
                Created = DateTime.Now,
                Title = "Vysocina",
                Attachments =
                {
                    new Attachment()
                    {
                        FileType = FileType.Picture,
                        FileSize = 42,
                        File = new byte[5],
                        Name = "Kuraptomuj"
                    }
                },
                Tags =
                {
                    user2
                },
                Team = team
            };
            dbContext.Posts.Add(post);

            var comment = new Comment
            {
                Author = user1,
                Text = "Velmi dobre to ide.",
                Created = DateTime.Now,
                Post = post,
                Tags =
                {
                    user2
                }
            };
            dbContext.Comments.Add(comment);
            var comment2 = new Comment
            {
                Author = user1,
                Text = "Asi hej",
                Created = DateTime.Now,
                Post = post,
            };
            dbContext.SaveChanges();
            dbContext.Users.Remove(user1);
            dbContext.Teams.Remove(team);
            dbContext.SaveChanges();
        }

        private static void ClearDatabase(ProjectDbContext dbContext)
        {
            dbContext.RemoveRange(dbContext.Attachments);
            dbContext.RemoveRange(dbContext.Comments);
            dbContext.RemoveRange(dbContext.Posts);
            dbContext.RemoveRange(dbContext.Teams);
            dbContext.RemoveRange(dbContext.Users);
            dbContext.RemoveRange(dbContext.UsersInTeams);
            dbContext.SaveChanges();
        }

        private static ProjectDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProjectDbContext>();
            optionsBuilder.UseSqlServer(
                @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog = ProjectDB;MultipleActiveResultSets = True;Integrated Security = True; ");
            return new ProjectDbContext(optionsBuilder.Options);
        }
    }
}
