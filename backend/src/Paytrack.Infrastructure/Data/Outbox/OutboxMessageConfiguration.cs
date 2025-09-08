namespace Paytrack.Infrastructure.Data.Outbox;

internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(om => om.Id);

        builder.Property(om => om.Type)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(om => om.Payload)
            .IsRequired()
            .HasColumnType("jsonb");

        builder.Property(om => om.Headers)
            .IsRequired()
            .HasColumnType("jsonb");

        builder.Property(om => om.Timestamp);
    }
}
