using System;
using System.IO;
using System.Web;
using AdPlatforms.Services;
using AdPlatforms.Infra;
using AdPlatforms.Responses;
using AdPlatforms.Responses.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AdPlatforms.Endpoints;

public class Platform : EndpointGroupBase
{
	public override void Map(WebApplication app)
	{
		var group = app.MapGroup("platforms");
		
		group.MapPost(string.Empty, SavePlatforms)
			.WithSummary("Saves platforms and locations")
			.Produces(StatusCodes.Status201Created)
			.Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status415UnsupportedMediaType)
			.DisableAntiforgery();
		
		group.MapGet("{*location}", GetPlatforms)
			.WithSummary("Retrieves platforms for a given location")
			.Produces<PlatformResponse>()
			.Produces<ErrorResponse>(StatusCodes.Status400BadRequest);
	}
	
	public static IResult SavePlatforms(IFormFile file, IPlatformService platformService)
	{
		if (!file.ContentType.Equals("text/plain", StringComparison.OrdinalIgnoreCase))
		{
			return CustomResults.UnsupportedMediaType();
		}
		
		using Stream readStream = file.OpenReadStream();
		
		var error = platformService.Save(readStream);
		
		return error is null
			? Results.Created()
			: Results.BadRequest(new ErrorResponse(error));
	}
	
	public static IResult GetPlatforms(string location, IPlatformService platformService)
	{
		location = HttpUtility.UrlDecode(location);
		
		return platformService.Get(location)
			.Match<IResult>(
				platforms => Results.Ok(new PlatformResponse(location, platforms)),
				error => Results.BadRequest(new ErrorResponse(error)));
	}
}