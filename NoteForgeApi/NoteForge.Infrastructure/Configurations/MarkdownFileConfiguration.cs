using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteForge.Domain;

namespace NoteForge.Infrastructure.Configurations
{
    public class MarkdownFileConfiguration : AuditableEntityConfiguration<MarkdownFile>
    {
        public override void Configure(EntityTypeBuilder<MarkdownFile> builder)
        {
            base.Configure(builder);

            builder.HasKey(x => x.Id);
            builder.Property(x => x.BlobName).HasMaxLength(200);
            builder.Property(x => x.FileName).HasMaxLength(200);

            builder
                .HasOne(x => x.AppUser)
                .WithMany(au => au.MarkdownFiles);
        }
    }
}
