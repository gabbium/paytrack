using Paytrack.Domain.Entities;

namespace Paytrack.Infrastructure.Data.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(t =>
        {
            t.HasCheckConstraint("ck_user_currency_iso", "char_length(currency) = 3 AND currency = upper(currency)");
        });

        builder.HasKey(u => u.Id);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.Email)
            .HasColumnType("citext")
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(u => u.PasswordHash)
            .HasMaxLength(512)
            .IsRequired();

        builder.OwnsOne(u => u.Preferences, prefs =>
        {
            prefs.Property(p => p.Currency)
                .HasColumnName("currency")
                .HasMaxLength(3)
                .IsRequired();

            prefs.Property(p => p.TimeZone)
                .HasColumnName("time_zone")
                .HasMaxLength(64)
                .IsRequired();
        });

        builder.Property(u => u.CreatedOn);

        builder.Property(u => u.CreatedBy);

        builder.Property(u => u.LastModifiedOn);

        builder.Property(u => u.LastModifiedBy);
    }
}