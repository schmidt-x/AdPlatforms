using AdPlatforms.Domain.Enums;

namespace AdPlatforms.Domain.Models;

public record Error(ErrorCode Code, string Description);
