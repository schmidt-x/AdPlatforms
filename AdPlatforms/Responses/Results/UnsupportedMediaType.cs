using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AdPlatforms.Responses.Results;

public class UnsupportedMediaTypeResult : IResult
{
	public Task ExecuteAsync(HttpContext httpContext)
	{
		httpContext.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;
		httpContext.Response.Headers.Append("Accept-Post", "text/plain");
		return Task.CompletedTask;
	}
}