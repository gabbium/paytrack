namespace Paytrack.Api.FunctionalTests.Assertions;

public static class HttpResponseMessageAssertions
{
    public static async Task<T> ShouldBeCreatedWithBodyAndLocation<T>(
        this HttpResponseMessage response,
        Func<T, string> expectedLocationPath)
    {
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var body = await response.Content.ReadFromJsonAsync<T>(TestConstants.Json);
        Assert.NotNull(body);

        var location = response.Headers.Location;
        location.ShouldNotBeNull();
        location.LocalPath.ShouldBe(expectedLocationPath(body));

        return body;
    }

    public static async Task<T> ShouldBeOkWithBody<T>(this HttpResponseMessage response)
    {
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<T>(TestConstants.Json);
        Assert.NotNull(body);

        return body;
    }

    public static void ShouldBeNoContent(this HttpResponseMessage response)
    {
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    public static async Task ShouldBeBadRequestWithValidation(this HttpResponseMessage response)
    {
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(TestConstants.Json);
        problem.ShouldNotBeNull();
    }

    public static async Task ShouldBeBadRequestWithProblem(this HttpResponseMessage response)
    {
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(TestConstants.Json);
        problem.ShouldNotBeNull();
    }

    public static async Task ShouldBeNotFoundWithProblem(this HttpResponseMessage response)
    {
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(TestConstants.Json);
        problem.ShouldNotBeNull();
    }

    public static async Task ShouldBeConflictWithProblem(this HttpResponseMessage response)
    {
        response.StatusCode.ShouldBe(HttpStatusCode.Conflict);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(TestConstants.Json);
        problem.ShouldNotBeNull();
    }

    public static void ShouldBeUnauthorizedWithBearerChallenge(this HttpResponseMessage response)
    {
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        response.Headers.WwwAuthenticate.ShouldContain(
            h => string.Equals(h.Scheme, "Bearer", StringComparison.OrdinalIgnoreCase));
    }

    public static async Task ShouldBeUnauthorizedWithProblem(this HttpResponseMessage response)
    {
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(TestConstants.Json);
        problem.ShouldNotBeNull();
    }
}
