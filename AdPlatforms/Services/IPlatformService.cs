using System.Collections.Generic;
using System.IO;
using AdPlatforms.Common;
using AdPlatforms.Domain.Models;

namespace AdPlatforms.Services;

public interface IPlatformService
{
	public Error? Save(Stream stream);
	public Result<IReadOnlyCollection<string>, Error> Get(string location);
}