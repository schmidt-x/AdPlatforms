using System.Collections.Generic;

namespace AdPlatforms.Domain.Models;

public record Platform(string Name, params IReadOnlyCollection<Location> Locations);
