using STC.Shared.Utilities.Response.Abstracts;

namespace STC.Auth.WebAPI;

public class ResponseGenerator(IResponse response) : IResult
{
    public async Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = (int)response.StatusCode;

        await httpContext.Response.WriteAsJsonAsync(response);
    }
}