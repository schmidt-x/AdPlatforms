using System.Collections.Generic;

namespace AdPlatforms.Responses;

public record PlatformResponse(string Location, IReadOnlyCollection<string> Platforms);