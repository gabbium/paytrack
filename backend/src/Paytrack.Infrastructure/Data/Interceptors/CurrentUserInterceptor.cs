using Paytrack.Application.Common.Interfaces;

namespace Paytrack.Infrastructure.Data.Interceptors;

public sealed class CurrentUserInterceptor(ICurrentUser currentUser) : DbConnectionInterceptor
{
    public override async Task ConnectionOpenedAsync(
        DbConnection connection,
        ConnectionEndEventData eventData,
        CancellationToken cancellationToken = default)
    {
        if (currentUser.IsAuthenticated)
        {
            using var command = connection.CreateCommand();
            command.CommandText = $"SET app.user_id = '{currentUser.UserId}'";
            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        await base.ConnectionOpenedAsync(connection, eventData, cancellationToken);
    }
}
