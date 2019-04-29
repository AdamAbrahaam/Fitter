﻿using System;
using System.Collections.Generic;
using System.Linq;
using Fitter.BL.Factories;
using Fitter.BL.Model;
using Fitter.BL.Repositories;
using Xunit;

namespace Fitter.BL.Tests
{
    public class CommentsRepositoryTests
    {
        [Fact]
        public void CreateComment()
        {
            var sut = CreateSUT();
            var post = new PostModel()
            {
                Id = Guid.NewGuid(),
                Author = new UserDetailModel()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Harry",
                    LastName = "Callum",
                    Email = "harry@callum.com",
                    Password = "asdf131"
                },
                Created = DateTime.Now.ToString("MM/dd/yyyy HH:mm"),
                Team = new TeamDetailModel()
                {
                    Id = Guid.NewGuid(),
                    Admin = new UserDetailModel()
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Harry",
                        LastName = "Callum",
                        Email = "harry@callum.com",
                        Password = "asdf131"
                    },
                    Description = "Harry Callum Team",
                    Name = "United"
                },
                Text = "Post in harry callum team",
                Title = "Main Title"
            };
            var model = new CommentModel()
            {
                Id = Guid.NewGuid(),
                Author = new UserDetailModel()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Oliver",
                    LastName = "Jake",
                    Email = "olijake@outlook.com",
                    Password = "oli12345"
                },
                Created = DateTime.Now.ToString("MM/dd/yyyy HH:mm"),
                Post = post,
                Text = "Comment on post in team"
            };
            sut.Create(model);
            Assert.NotNull(sut.GetCommentsForPost(post.Id));
            
        }

        [Fact]
        public void DeleteComment()
        {
            var sut = CreateSUT();
            var post = new PostModel()
            {
                Id = Guid.NewGuid(),
                Author = new UserDetailModel()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "James",
                    LastName = "Charlie",
                    Email = "james@charlie.com",
                    Password = "charlie258"
                },
                Created = DateTime.Now.ToString("MM/dd/yyyy HH:mm"),
                Team = new TeamDetailModel()
                {
                    Id = Guid.NewGuid(),
                    Admin = new UserDetailModel()
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "William",
                        LastName = "Damian",
                        Email = "william@damian.com",
                        Password = "password"
                    },
                    Description = "William Damian Team",
                    Name = "Manchester"
                },
                Text = "Best footbal club",
                Title = "Football"
            };

            var model = new CommentModel()
            {
                Id = Guid.NewGuid(),
                Author = new UserDetailModel()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Oliver",
                    LastName = "Jake",
                    Email = "olijake@outlook.com",
                    Password = "oli12345"
                },
                Created = DateTime.Now.ToString("MM/dd/yyyy HH:mm"),
                Post = post,
                Text = "Comment on post in team"
            };
            sut.Create(model);
            sut.Delete(model.Id);
            Assert.Empty(sut.GetCommentsForPost(post.Id));
        }

        [Fact]
        public void GetCommentsInPost()
        {
            var sut = CreateSUT();
            var user = new UserDetailModel()
            {
                Id = Guid.NewGuid(),
                FirstName = "Oscar",
                LastName = "Rhys",
                Email = "oscar@rhys.com",
                Password = "password"
            };
            var post = new PostModel()
            {
                Id = Guid.NewGuid(),
                Author = new UserDetailModel()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Thomas",
                    LastName = "Joe",
                    Email = "thomas@joe.com",
                    Password = "password"
                },
                Created = DateTime.Now.ToString("MM/dd/yyyy HH:mm"),
                Team = new TeamDetailModel()
                {
                    Id = Guid.NewGuid(),
                    Admin = user,
                    Description = "Jacob Jacob Team",
                    Name = "Sea Dogs"
                },
                Text = "Post in Jacob Jacob team",
                Title = "Main Title of post"
            };

            var model = new CommentModel()
            {
                Id = Guid.NewGuid(),
                Author = user,
                Created = DateTime.Now.ToString("MM/dd/yyyy HH:mm"),
                Post = post,
                Text = "Unbelievable comment"
            };

            sut.Create(model);
            var retrievedComment = sut.GetCommentsForPost(post.Id);
            Assert.Single(retrievedComment);
        }

        private CommentsRepository CreateSUT()
        {
            return new CommentsRepository(new InMemoryDbContext(), new Mapper.Mapper());
        }
    }
}