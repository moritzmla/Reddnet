using BlogCoreEngine.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCoreEngine.DataAccess.Data
{
    public static class ApplicationSeed
    {
        private static Guid authorId => Guid.Parse("c6248606-ed80-4c2e-89b3-353d183fe284");
        private static Guid blogId => Guid.Parse("22795939-9e17-492e-a890-9234ca8f5a08");
        private static Guid postId => Guid.Parse("f11fe6c8-0d5b-4c6b-9673-035d47d4396e");
        private static Guid adminId => Guid.Parse("37a0a393-bacf-48c3-b947-c45c53d467ee");
        private static Guid adminRoleId => Guid.Parse("7e2a48a9-ffeb-4b9c-bf87-4990ea281ec7");

        public static void ApplySeed(ModelBuilder builder)
        {
            builder.Entity<Author>().HasData(Authors());
            builder.Entity<BlogDataModel>().HasData(Blogs());
            builder.Entity<PostDataModel>().HasData(Posts());
            builder.Entity<CommentDataModel>().HasData(Comments());
            builder.Entity<OptionDataModel>().HasData(Options());
            builder.Entity<ApplicationUser>().HasData(ApplicationUsers());
            builder.Entity<IdentityRole>().HasData(Roles());
            builder.Entity<IdentityUserRole<string>>().HasData(UserRoles());
        }

        private static List<Author> Authors()
        {
            return new List<Author>
            {
                new Author
                {
                    Id = authorId,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    Image = System.IO.File.ReadAllBytes(".//wwwroot//images//Profile.png"),
                    Name = "Admin"
                }
            };
        }

        private static List<BlogDataModel> Blogs()
        {
            return new List<BlogDataModel>
            {
                new BlogDataModel
                {
                    Id = blogId,
                    Cover = System.IO.File.ReadAllBytes(".//wwwroot//images//Logo.png"),
                    Created = DateTime.Now,
                    Description = "default description",
                    Name = "default subreddit",
                    Modified = DateTime.Now
                }
            };
        }

        private static List<PostDataModel> Posts()
        {
            return new List<PostDataModel>
            {
                new PostDataModel
                {
                    Id = postId,
                    AuthorId = authorId,
                    BlogId = blogId,
                    Archieved = true,
                    Pinned = true,
                    Content = "Default Text",
                    Title = "Default Title",
                    Cover = System.IO.File.ReadAllBytes(".//wwwroot//images//Default.png"),
                    Views = 120,
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                }
            };
        }

        private static List<CommentDataModel> Comments()
        {
            return new List<CommentDataModel>
            {
                new CommentDataModel
                {
                    Id = Guid.NewGuid(),
                    PostId = postId,
                    AuthorId = authorId,
                    Content = "Default Comment",
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                }
            };
        }

        private static List<OptionDataModel> Options()
        {
            return new List<OptionDataModel>
            {
                new OptionDataModel
                {
                    Id = Guid.NewGuid(),
                    Title = "Reddnet",
                    Logo = System.IO.File.ReadAllBytes(".//wwwroot//images//Logo.png")
                }
            };
        }

        private static List<IdentityRole> Roles()
        {
            return new List<IdentityRole>
            {
                new IdentityRole {
                    Id = adminRoleId.ToString(),
                    Name = "Administrator",
                    NormalizedName = "administrator"
                }
            };
        }

        private static List<ApplicationUser> ApplicationUsers()
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            return new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = adminId.ToString(),
                    AuthorId = authorId,
                    UserName = "Admin",
                    NormalizedUserName = "admin",
                    Email = "default@default.com",
                    NormalizedEmail = "default@default.com",
                    PasswordHash = hasher.HashPassword(null, "adminPassword"),
                    SecurityStamp = string.Empty,
                    EmailConfirmed = true
                }
            };
        }

        private static List<IdentityUserRole<string>> UserRoles()
        {
            return new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    UserId = adminId.ToString(),
                    RoleId = adminRoleId.ToString()
                }
            };
        }
    }
}
