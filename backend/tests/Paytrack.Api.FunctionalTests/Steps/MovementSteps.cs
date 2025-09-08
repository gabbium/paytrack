using Paytrack.Application.UseCases.Movements.Commands.CreateMovement;
using Paytrack.Application.UseCases.Movements.Commands.UpdateMovement;
using Paytrack.Application.UseCases.Movements.Contracts;
using Paytrack.Application.UseCases.Movements.Queries.ListMovements;

namespace Paytrack.Api.FunctionalTests.Steps;

public sealed class MovementSteps(TestFixture fx)
{
    public CreateMovementCommand Given_ValidCreateCommand()
    {
        return new CreateMovementCommandBuilder().Build();
    }

    public CreateMovementCommand Given_InvalidCreateCommand_TooLongDescription()
    {
        return new CreateMovementCommandBuilder()
                .WithDescription(new string('a', 129))
                .Build();
    }

    public UpdateMovementCommand Given_ValidUpdateCommand(Guid id)
    {
        return new UpdateMovementCommandBuilder()
                .WithId(id)
                .Build();
    }

    public UpdateMovementCommand Given_InvalidUpdateCommand_TooLongDescription(Guid id)
    {
        return new UpdateMovementCommandBuilder()
                .WithId(id)
                .WithDescription(new string('a', 129))
                .Build();
    }

    public ListMovementsQuery Given_ValidListQuery()
    {
        return new ListMovementsQueryBuilder()
            .Build();
    }

    public ListMovementsQuery Given_InvalidValidListQuery_PageNumberNegative()
    {
        return new ListMovementsQueryBuilder()
            .WithPageNumber(-1)
            .Build();
    }

    public async Task<MovementResponse> Given_ExistingMovement()
    {
        var command = new CreateMovementCommandBuilder().Build();

        var response = await fx.Client.PostAsJsonAsync("/api/movements", command);
        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadFromJsonAsync<MovementResponse>(TestConstants.Json);
        body.ShouldNotBeNull();

        return body;
    }

    public async Task<HttpResponseMessage> When_AttemptToList(ListMovementsQuery query)
    {
        var queryParams = QueryString.Create(new Dictionary<string, string?>
        {
            ["pageNumber"] = query.PageNumber.ToString(),
            ["pageSize"] = query.PageSize.ToString()
        });

        return await fx.Client.GetAsync("/api/movements" + queryParams);
    }

    public async Task<HttpResponseMessage> When_AttemptToGetById(Guid id)
    {
        return await fx.Client.GetAsync("/api/movements/" + id);
    }
    public async Task<HttpResponseMessage> When_AttemptToCreate(CreateMovementCommand command)
    {
        return await fx.Client.PostAsJsonAsync("/api/movements", command);
    }

    public async Task<HttpResponseMessage> When_AttemptToUpdate(Guid id, UpdateMovementCommand command)
    {
        return await fx.Client.PutAsJsonAsync("/api/movements/" + id, command);
    }

    public async Task<HttpResponseMessage> When_AttemptToDelete(Guid id)
    {
        return await fx.Client.DeleteAsync("/api/movements/" + id);
    }
}
