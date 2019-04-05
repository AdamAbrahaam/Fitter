﻿using System;
using System.Collections.Generic;
using System.Linq;
using Fitter.DAL.Entity;
using Fitter.DAL.Enums;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fitter.DAL.Tests
{
    public class InMemoryFitterRelationsTests
    {
        public class FitterDbContextTests
        {
            private IFitterDbContext dbContextFitter;
            public FitterDbContextTests()
            {
                dbContextFitter = new InMemoryFitterDbContext();
            }

            [Fact]
            public void AddUserToTeam()
            {
                var userAdam = new User()
                {
                    FirstName = "Adam",
                    LastName = "Zly",
                    Email = "adamko@seznam.cz",
                    Password = "asdfg"
                };

                var userEva = new User()
                {
                    FirstName = "Eva",
                    LastName = "Vianocna",
                    Email = "evicka128@seznam.cz",
                    Password = "123456"
                };

                var team1 = new Team()
                {
                    Description = "Skupina Jozefa Dlheho",
                    Name = "The bests",
                    UsersInTeams = new List<UsersInTeam>()
                    {
                        new UsersInTeam { User = userEva},
                        new UsersInTeam { User = userAdam}
                    }
                };

                using (var dbContext = dbContextFitter.CreateDbContext())
                {
                    dbContext.Users.Add(userEva);
                    dbContext.Users.Add(userAdam);
                    dbContext.Teams.Add(team1);
                    dbContext.SaveChanges();
                }

                using (var dbContext = dbContextFitter.CreateDbContext())
                {
                    var retrievedTeam = dbContext.Teams
                        .Include(x => x.UsersInTeams)
                        .First(x => x.Id == team1.Id);
                    Assert.Equal(2, retrievedTeam.UsersInTeams.Count);
                }
            }


            [Fact]
            public void DeleteUsersFromTeam()
            {
                var userAdam = new User()
                {
                    FirstName = "Adam",
                    LastName = "Zly",
                    Email = "adamko@seznam.cz",
                    Password = "asdfg"
                };

                var userEva = new User()
                {
                    FirstName = "Eva",
                    LastName = "Vianocna",
                    Email = "evicka128@seznam.cz",
                    Password = "123456"
                };

                var team1 = new Team()
                {
                    Description = "Skupina Adam a Eva",
                    Name = "Adam & Eva",
                    UsersInTeams = new List<UsersInTeam>()
                    {
                        new UsersInTeam { User = userEva},
                        new UsersInTeam { User = userAdam}
                    }
                };

                using (var dbContext = dbContextFitter.CreateDbContext())
                {
                    dbContext.Users.Add(userEva);
                    dbContext.Users.Add(userAdam);
                    dbContext.Teams.Add(team1);
                    dbContext.SaveChanges();
                }

                using (var dbContext = dbContextFitter.CreateDbContext())
                {
                    dbContext.Remove(userAdam);
                    dbContext.Remove(userEva);
                    dbContext.SaveChanges();
                }

                using (var dbContext = dbContextFitter.CreateDbContext())
                {
                    var retrievedTeam = dbContext.Teams
                        .Include(x => x.UsersInTeams)
                        .FirstOrDefault(x => x.Id == team1.Id);
                    Assert.Equal(0, retrievedTeam.UsersInTeams.Count);
                }
            }

            [Fact]
            public void SetAdmin()
            {
                var admin = new User()
                {
                    FirstName = "Jozef",
                    LastName = "Dlhy",
                    Email = "jozefdlhy@gmail.com",
                    Password = "asdfg123"
                };

                var team = new Team()
                {
                    Admin = admin,
                    Description = "Jozefova skupina",
                    Name = "United"
                };

                using (var dbContext = dbContextFitter.CreateDbContext())
                {
                    dbContext.Users.Add(admin);
                    dbContext.Teams.Add(team);
                    dbContext.SaveChanges();
                }

                using (var dbContext = dbContextFitter.CreateDbContext())
                {
                    var retrievedTeam = dbContext.Teams
                        .Include(u => u.UsersInTeams)
                        .Include(a => a.Admin)
                        .First(x => x.Id == team.Id);
                    Assert.Equal(retrievedTeam.Admin.Id, admin.Id);
                }
            }

            [Fact]
            public void AddPost()
            {
                var userMichaela = new User()
                {
                    LastName = "Michaela",
                    FirstName = "Velka",
                    Email = "misicka@gmail.com",
                    Password = "abcdefgh"
                };

                var userDaniel = new User()
                {
                    LastName = "Daniel",
                    FirstName = "Maly",
                    Email = "danko@gmail.com",
                    Password = "13465798"
                };

                var postInMichaelaTeam = new Post()
                {
                    Author = userDaniel,
                    Created = DateTime.Today,
                    Text = "Skuska postu",
                    Title = "Obrazok",
                };

                var teamMichaela = new Team()
                {
                    Admin = userMichaela,
                    Description = "Spolocna skupina pre dievcata a chlapcov",
                    Name = "Girls&Boys",
                    Posts = new List<Post>() { postInMichaelaTeam }
                };

                using (var dbContext = dbContextFitter.CreateDbContext())
                {
                    dbContext.Users.Add(userMichaela);
                    dbContext.Users.Add(userDaniel);
                    dbContext.Teams.Add(teamMichaela);
                    dbContext.Posts.Add(postInMichaelaTeam);
                    dbContext.SaveChanges();
                }

                using (var dbContext = dbContextFitter.CreateDbContext())
                {
                    var retrievedTeam = dbContext.Teams
                        .Include(x => x.UsersInTeams)
                        .Include(p => p.Posts)
                        .First(x => x.Id == teamMichaela.Id);
                    Assert.Equal(1,retrievedTeam.Posts.Count);
                }
            }

            [Fact]
            public void AddComment()
            {
                var userDaniel = new User()
                {
                    LastName = "Daniel",
                    FirstName = "Maly",
                    Email = "danko@gmail.com",
                    Password = "13465798"
                };

                var postInMichaelaTeam = new Post()
                {
                    Author = userDaniel,
                    Created = DateTime.Today,
                    Text = "Post v Michaelovej skupine",
                    Title = "Post",
                };

                var teamMichaela = new Team()
                {
                    Description = "Spolocna skupina pre dievcata a chlapcov",
                    Name = "Girls&Boys",
                    Posts = new List<Post>() { postInMichaelaTeam }
                };

                var commentInMichaelaTeam = new Comment()
                {
                    Author = userDaniel,
                    Post = postInMichaelaTeam,
                    Text = "Komentar na post",
                    Created = new DateTime(2019, 4, 4)
                };

                using (var dbContext = dbContextFitter.CreateDbContext())
                {
                    dbContext.Users.Add(userDaniel);
                    dbContext.Teams.Add(teamMichaela);
                    dbContext.Posts.Add(postInMichaelaTeam);
                    dbContext.SaveChanges();
                }

                using (var dbContext = dbContextFitter.CreateDbContext())
                {
                    var retrievedComment = dbContext.Posts
                        .Include(c => c.Comments)
                        .Include(t => t.Team)
                        .First(x => x.Id == postInMichaelaTeam.Id);
                    Assert.NotNull(retrievedComment);
                }
            }

            [Fact]
            public void AddTag()
            {
                var userAlfonz = new User()
                {
                    LastName = "Kruty",
                    FirstName = "Alfonz",
                    Email = "kruty@gmail.com",
                    Password = "qwerty"
                };

                var userDaniel = new User()
                {
                    LastName = "Daniel",
                    FirstName = "Maly",
                    Email = "danko@gmail.com",
                    Password = "13465798"
                };

                var postInTeam = new Post()
                {
                    Author = userDaniel,
                    Created = DateTime.Today,
                    Text = "Text ako post",
                    Title = "Skuska",
                    Tags = new List<User>() { userDaniel }
                };

                var team = new Team()
                {
                    Description = "Team pre Daniela a Alfonza",
                    Name = "A & D",
                    Posts = new List<Post>() { postInTeam }
                };

                using (var dbContext = dbContextFitter.CreateDbContext())
                {
                    dbContext.Users.Add(userAlfonz);
                    dbContext.Users.Add(userDaniel);
                    dbContext.Teams.Add(team);
                    dbContext.Posts.Add(postInTeam);
                    dbContext.SaveChanges();
                }

                using (var dbContext = dbContextFitter.CreateDbContext())
                {
                    var retrievedPost = dbContext.Posts
                        .Include(t => t.Tags)
                        .Include(t => t.Team)
                        .Include(a => a.Author)
                        .First(x => x.Id == postInTeam.Id);
                    Assert.Equal(1, retrievedPost.Tags.Count);
                }
            }

            [Fact]
            public void AddAttachment()
            {
                var userRichard = new User()
                {
                    LastName = "Richard",
                    FirstName = "Smutny",
                    Email = "richi@gmail.com",
                    Password = "qwertz"
                };

                var attachment = new Attachment()
                {
                    FileType = FileType.Picture,
                    File = new byte[2],
                    Name = "Obrazok Brna"
                };

                var postInTeam = new Post()
                {
                    Author = userRichard,
                    Created = DateTime.Today,
                    Text = "Post na subor PDF",
                    Title = "PDF",
                    Attachments = new List<Attachment>() { attachment}
                };

                var team = new Team()
                {
                    Description = "Team pre Daniela a Alfonza",
                    Name = "A & D",
                    Posts = new List<Post>() { postInTeam }
                };

                using (var dbContext = dbContextFitter.CreateDbContext())
                {

                    dbContext.Users.Add(userRichard);
                    dbContext.Teams.Add(team);
                    dbContext.Posts.Add(postInTeam);
                    dbContext.SaveChanges();
                }

                using (var dbContext = dbContextFitter.CreateDbContext())
                {
                    var retrievedPost = dbContext.Posts
                        .Include(a => a.Attachments)
                        .Include(t => t.Team)
                        .Include(a => a.Author)
                        .First(x => x.Id == postInTeam.Id);
                    Assert.Equal(1, retrievedPost.Attachments.Count);
                }
            }

            [Fact]
            public void RemovePost()
            {
                var userRichard = new User()
                {
                    LastName = "Richard",
                    FirstName = "Smutny",
                    Email = "richi@gmail.com",
                    Password = "qwertz"
                };

                var post = new Post()
                {
                    Author = userRichard,
                    Created = DateTime.Today,
                    Title = "Nahodny post",
                    Text = "Nahodny text k postu v skupine"
                };

                var team = new Team()
                {
                    Admin = userRichard,
                    Description = "Team pre Daniela a Alfonza",
                    Name = "A & D",
                    Posts = new List<Post>() { post}
                };

                using (var dbContext = dbContextFitter.CreateDbContext())
                {

                    dbContext.Users.Add(userRichard);
                    dbContext.Teams.Add(team);
                    dbContext.SaveChanges();
                }

                using (var dbContext = dbContextFitter.CreateDbContext())
                {
                    dbContext.Remove(post);
                    dbContext.SaveChanges();
                }

                using (var dbContext = dbContextFitter.CreateDbContext())
                {
                    var retrievedTeam = dbContext.Teams
                        .Include(x => x.UsersInTeams)
                        .Include(p => p.Posts)
                        .FirstOrDefault(x => x.Id == team.Id);
                    Assert.Equal(0, retrievedTeam.Posts.Count);
                }
            }

            [Fact]
            public void RemovePostAndComment()
            {
                var comment = new Comment()
                {
                    Text = "Koment k postu",
                    Created = DateTime.Today
                };

                var postInMichaelaTeam = new Post()
                {
                    Created = DateTime.Today,
                    Text = "Post v Michaelovej skupine",
                    Title = "Post",
                    Comments = new List<Comment>() { comment}
                };

                var teamMichaela = new Team()
                {
                    Description = "Spolocna skupina pre dievcata a chlapcov",
                    Name = "Girls&Boys",
                    Posts = new List<Post>() { postInMichaelaTeam }
                };

                using (var dbContext = dbContextFitter.CreateDbContext())
                {
                    dbContext.Teams.Add(teamMichaela);
                    dbContext.Comments.Add(comment);
                    dbContext.Posts.Add(postInMichaelaTeam);
                    dbContext.SaveChanges();
                }

                using (var dbContext = dbContextFitter.CreateDbContext())
                {
                    dbContext.Posts.Remove(postInMichaelaTeam);
                    dbContext.SaveChanges();
                }

                using (var dbContext = dbContextFitter.CreateDbContext())
                {
                    var retrievedComment = dbContext.Comments
                        .FirstOrDefault(x => x.Id == postInMichaelaTeam.Id);
                    Assert.Null(retrievedComment);
                }
            }
        }
    }
}