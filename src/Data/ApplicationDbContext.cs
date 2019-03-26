using Reddnet.Data.AccountData;
using Reddnet.Models.DataModels;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reddnet.Data.ApplicationData
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<SettingDataModel> Settings { get; set; }
        public DbSet<PostDataModel> Posts { get; set; }
        public DbSet<CommentDataModel> Comments { get; set; }
        public DbSet<BlogDataModel> Blogs { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Blog

            modelBuilder.Entity<BlogDataModel>().HasMany(x => x.Posts).WithOne(x => x.Blog);

            modelBuilder.Entity<BlogDataModel>().Property(x => x.Cover).HasDefaultValue(System.IO.File.ReadAllBytes(".//wwwroot//images//Default.png"));
            modelBuilder.Entity<BlogDataModel>().Property(x => x.Created).HasDefaultValue(DateTime.Now);
            modelBuilder.Entity<BlogDataModel>().Property(x => x.Name).HasDefaultValue("");
            modelBuilder.Entity<BlogDataModel>().Property(x => x.Description).HasDefaultValue("");

            // Post

            modelBuilder.Entity<PostDataModel>().HasMany(c => c.Comments).WithOne(b => b.Post);

            modelBuilder.Entity<PostDataModel>().Property(x => x.Pinned).HasDefaultValue(false);
            modelBuilder.Entity<PostDataModel>().Property(x => x.Archieved).HasDefaultValue(false);
            modelBuilder.Entity<PostDataModel>().Property(x => x.Cover).HasDefaultValue(System.IO.File.ReadAllBytes(".//wwwroot//images//Default.png"));
            modelBuilder.Entity<PostDataModel>().Property(x => x.LastChangeDate).HasDefaultValue(DateTime.Now);
            modelBuilder.Entity<PostDataModel>().Property(x => x.UploadDate).HasDefaultValue(DateTime.Now);
            modelBuilder.Entity<PostDataModel>().Property(x => x.Content).HasDefaultValue("");
            modelBuilder.Entity<PostDataModel>().Property(x => x.Title).HasDefaultValue("");
            modelBuilder.Entity<PostDataModel>().Property(x => x.Tags).HasDefaultValue("");
            modelBuilder.Entity<PostDataModel>().Property(x => x.Preview).HasDefaultValue("");
            modelBuilder.Entity<PostDataModel>().Property(x => x.Link).HasDefaultValue("");
            modelBuilder.Entity<PostDataModel>().Property(x => x.Views).HasDefaultValue(0);

            // Comment

            modelBuilder.Entity<CommentDataModel>().Property(x => x.Content).HasDefaultValue("");
            modelBuilder.Entity<CommentDataModel>().Property(x => x.UploadDate).HasDefaultValue(DateTime.Now);

            // ApplicationUser

            modelBuilder.Entity<ApplicationUser>().HasMany(c => c.Posts).WithOne(a => a.Author);
            modelBuilder.Entity<ApplicationUser>().HasMany(c => c.Comments).WithOne(a => a.Author);

            modelBuilder.Entity<ApplicationUser>().Property(x => x.Image).HasDefaultValue(System.IO.File.ReadAllBytes(".//wwwroot//images//ProfilPicture.png"));

        }
    }
}
