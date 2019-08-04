using BlogCoreEngine.Core.Entities;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogCoreEngine.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<OptionDataModel> Options { get; set; }
        public DbSet<PostDataModel> Posts { get; set; }
        public DbSet<CommentDataModel> Comments { get; set; }
        public DbSet<BlogDataModel> Blogs { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ApplicationSeed.ApplySeed(builder);

            builder.Entity<ApplicationUser>(ConfigurateApplicationUser);
            builder.Entity<BlogDataModel>(ConfigurateBlog);
            builder.Entity<PostDataModel>(ConfiguratePost);
            builder.Entity<Author>(ConfigurateAuthor);
            builder.Entity<CommentDataModel>(ConfigurateComment);
            builder.Entity<OptionDataModel>(ConfigurateOption);
        }

        private void ConfigurateAuthor(EntityTypeBuilder<Author> builder)
        {
            builder.HasMany(c => c.Posts)
                .WithOne(a => a.Author)
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(c => c.Comments)
                .WithOne(a => a.Author)
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(x => x.Image)
                .HasDefaultValue(System.IO.File.ReadAllBytes(".//wwwroot//images//Profile.png"));
        }

        private void ConfigurateBlog(EntityTypeBuilder<BlogDataModel> builder)
        {
            builder.HasMany(x => x.Posts)
                .WithOne(x => x.Blog)
                .HasForeignKey(x => x.BlogId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(x => x.Cover)
                .HasDefaultValue(System.IO.File.ReadAllBytes(".//wwwroot//images//Default.png"));
        }

        private void ConfigurateComment(EntityTypeBuilder<CommentDataModel> builder)
        {
        }

        private void ConfigurateOption(EntityTypeBuilder<OptionDataModel> builder)
        {
        }

        private void ConfigurateApplicationUser(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasOne(x => x.Author)
                .WithOne()
                .OnDelete(DeleteBehavior.SetNull);
        }

        private void ConfiguratePost(EntityTypeBuilder<PostDataModel> builder)
        {
            builder.HasMany(c => c.Comments)
                .WithOne(b => b.Post)
                .HasForeignKey(x => x.PostId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(x => x.Pinned)
                .HasDefaultValue(false);

            builder.Property(x => x.Archieved)
                .HasDefaultValue(false);

            builder.Property(x => x.Cover)
                .HasDefaultValue(System.IO.File.ReadAllBytes(".//wwwroot//images//Default.png"));

            builder.Property(x => x.Views)
                .HasDefaultValue(0);
        }
    }
}
