namespace AdPlatforms.Responses;

public class ErrorResponse(string errorCode, string errorDescription)
{
	public string ErrorCode { get; } = errorCode;
	public string ErrorDescription { get; } = errorDescription;
}