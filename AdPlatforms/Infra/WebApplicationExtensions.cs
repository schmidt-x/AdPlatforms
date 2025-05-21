using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;

namespace AdPlatforms.Infra;

public static class WebApplicationExtensions
{
	public static WebApplication MapEndpoints(this WebApplication app)
	{
		var endpointGroupType = typeof(EndpointGroupBase);
		
		var endpointGroupTypes = Assembly
			.GetExecutingAssembly()
			.GetExportedTypes()
			.Where(t => t.IsSubclassOf(endpointGroupType));
		
		foreach (var type in endpointGroupTypes)
		{
			if (Activator.CreateInstance(type) is EndpointGroupBase instance)
			{
				instance.Map(app);
			}
		}
		
		return app;
	}
}