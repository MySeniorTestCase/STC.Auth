using STC.Shared.Utilities.Response.Abstracts;

namespace STC.Auth.WebAPI;

public class ResponseGenerator(IResponse response) : IResult
{
    public async Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = (int)response.StatusCode;

        object body = response is IDataResponse<object> dataResponse ? dataResponse : response;

        await httpContext.Response.WriteAsJsonAsync(body);
    }
}