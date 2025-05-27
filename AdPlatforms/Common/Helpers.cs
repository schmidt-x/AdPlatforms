using System;
using System.Collections.Generic;
using System.IO;
using AdPlatforms.Domain.Enums;
using AdPlatforms.Domain.Models;

namespace AdPlatforms.Common;

public static class Helpers
{
	public static Result<IReadOnlyCollection<Platform>, Error> ParsePlatforms(TextReader textReader)
	{
		List<Platform> platforms = [];
		
		int lineIndex = 0;
		while (textReader.ReadLine() is { } rawLine)
		{
			lineIndex++;
			ReadOnlySpan<char> line = rawLine;
			
			line = line.Trim();
			if (line.IsEmpty) continue;
			
			int index = line.IndexOf(':');
			if (index == -1)
			{
				return new Error(ErrorCode.InvalidValue, $"Platform and Location(s) must be separated by a colon ':'. Line: {lineIndex}.");
			}
			
			var platform = line[..index].TrimEnd();
			if (platform.IsEmpty)
			{
				return new Error(ErrorCode.EmptyPlatform, $"Platform name is required. Line: {lineIndex}.");
			}
			
			var rawLocations = line[(index+1)..];
			if (rawLocations.IsEmpty)
			{
				return new Error(ErrorCode.EmptyLocation, $"At least one Location is required. Line: {lineIndex}.");
			}
			
			List<Location> locations = [];
			
			foreach (var locationRange in rawLocations.Split(','))
			{
				var rawLocation = rawLocations[locationRange].Trim();
				
				if (rawLocation.IsEmpty) continue; // Empty entries are allowed (e.g., "Platform:,,,/a/b/c,,").
				
				if (ParseLocation(rawLocation).IsError(out Error? error, out Location? location))
				{
					return error with { Description = $"{error.Description} Line: {lineIndex}." };
				}
				
				locations.Add(location);
			}
			
			if (locations.Count == 0)
			{
				return new Error(ErrorCode.EmptyLocation, $"At least one Location is required. Line: {lineIndex}.");
			}
			
			platforms.Add(new Platform(platform.ToString(), locations));
		}
		
		return platforms.Count != 0
			? platforms
			: new Error(ErrorCode.EmptyPlatform, "At least one Platform is required.");
	}
	
	public static Result<Location, Error> ParseLocation(ReadOnlySpan<char> location)
	{
		List<string> segments = [];
		
		foreach (var segmentRange in location.Split('/'))
		{
			var segment = location[segmentRange].Trim();
			
			if (segment.IsEmpty) continue; // Empty segments are allowed (e.g., "Platform:///a/b/c//").
			
			foreach (var ch in segment)
			{
				// TODO: validate for restricted characters
			}
			
			segments.Add(segment.ToString());
		}
		
		return segments.Count != 0
			? new Location(segments)
			: new Error(ErrorCode.EmptyLocation, "Location must contain at least one non-empty segment.");
	}
}