using System.Collections.Generic;

namespace AdPlatforms.Domain.Models;

public record Location(params IReadOnlyCollection<string> Segments);
