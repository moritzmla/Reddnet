using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reddnet.Core.Entities;

namespace Reddnet.DataAccess.Configuration
{
    class PostConfiguration : IEntityTypeConfiguration<PostEntity>
    {
        public void Configure(EntityTypeBuilder<PostEntity> builder)
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
