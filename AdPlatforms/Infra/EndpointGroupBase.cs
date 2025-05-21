using Microsoft.AspNetCore.Builder;

namespace AdPlatforms.Infra;

public abstract class EndpointGroupBase
{
	public abstract void Map(WebApplication app);
}