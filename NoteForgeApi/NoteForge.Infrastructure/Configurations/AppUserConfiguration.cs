using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteForge.Domain;

namespace NoteForge.Infrastructure.Configurations
{
    public class AppUserConfiguration : AuditableEntityConfiguration<AppUser>
    {
        public override void Configure(EntityTypeBuilder<AppUser> builder)
        {
            base.Configure(builder);

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
