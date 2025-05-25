using AdPlatforms.Domain.Models;
using AdPlatforms.Responses;

namespace AdPlatforms.Infra;

public static class ErrorExtensions
{
	public static ErrorResponse ToErrorResponse(this Error error) => new(error.Code.ToString(), error.Description);
}