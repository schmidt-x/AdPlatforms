using AdPlatforms.Domain.Models;

namespace AdPlatforms.Responses;

public class ErrorResponse(Error error)
{
	public string ErrorCode { get; } = error.Code.ToString();
	public string ErrorDescription { get; } = error.Description;
}