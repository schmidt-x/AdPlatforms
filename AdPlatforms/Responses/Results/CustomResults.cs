using Microsoft.AspNetCore.Http;

namespace AdPlatforms.Responses.Results;

public static class CustomResults
{
	public static IResult UnsupportedMediaType() => new UnsupportedMediaTypeResult();
}