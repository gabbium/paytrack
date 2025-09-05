using Paytrack.Domain.Entities;

namespace Paytrack.Infrastructure.Data.Configurations;

public sealed class MovementConfiguration : IEntityTypeConfiguration<Movement>
{
    public void Configure(EntityTypeBuilder<Movement> builder)
    {
        builder.ToTable(t =>
        {
            t.HasCheckConstraint("ck_movement_amount_positive", "amount > 0");
        });

        builder.HasKey(m => m.Id);

        builder.HasIndex(m => m.UserId);

        builder.Property(m => m.UserId);

        builder.Property(m => m.Kind)
            .HasColumnType("movement_kind");

        builder.Property(m => m.Amount)
            .HasPrecision(18, 2);

        builder.Property(m => m.Description)
            .HasMaxLength(128);

        builder.Property(m => m.OccurredOn);

        builder.Property(m => m.CreatedOn);

        builder.Property(m => m.CreatedBy);
        
        builder.Property(m => m.LastModifiedOn);
        
        builder.Property(m => m.LastModifiedBy);
    }
}
