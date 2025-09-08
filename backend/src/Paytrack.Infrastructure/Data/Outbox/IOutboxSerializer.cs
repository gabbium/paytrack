namespace Paytrack.Infrastructure.Data.Outbox;

public interface IOutboxSerializer
{
    string Serialize(object value);
}
