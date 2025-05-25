using System;
using System.Threading;
using System.Threading.Tasks;
using AdPlatforms.Responses;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace AdPlatforms.Infra;

public class GlobalExceptionHandler : IExceptionHandler
{
	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken ct)
	{
		var response = httpContext.Response;
		
		switch (exception)
		{
			case BadHttpRequestException badRequestEx:
				response.StatusCode = StatusCodes.Status400BadRequest;
				await response.WriteAsJsonAsync(new ErrorResponse("BadRequest", badRequestEx.Message), ct);
				break;
			
			default:
				response.StatusCode = StatusCodes.Status500InternalServerError;
				await response.WriteAsJsonAsync(new ErrorResponse("InternalServerError", "Something went wrong :)"), ct);
				break;
		}
		
		return true;
	}
}