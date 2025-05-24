using System.Collections.Generic;
using System.Diagnostics;
using AdPlatforms.Domain.Models;

namespace AdPlatforms.Common.Structures;

internal class PlatformTrie
{
	private readonly TrieNode _root = new(string.Empty);
	
	internal void Add(Platform platform)
	{
		foreach (var location in platform.Locations)
		{
			Debug.Assert(location.Segments.Count > 0, "Root node cannot have Platforms.");
			
			var current = _root;
			
			foreach (var segment in location.Segments)
			{
				if (current.Children.Count == 0 || !current.Children.TryGetValue(segment, out var child))
				{
					child = new TrieNode(segment);
					current.Children.Add(child.LocationSegment, child);
				}
				
				current = child;
			}
			
			current.Platforms.Add(platform.Name); // Here 'current' is the last node.
		}
	}
	
	internal IReadOnlyCollection<string> Find(Location location)
	{
		List<string> platforms = [];
		var current = _root;
		
		foreach (var segment in location.Segments)
		{
			if (current.Children.Count == 0 || !current.Children.TryGetValue(segment, out var child))
				break;
			
			platforms.AddRange(child.Platforms);
			current = child;
		}
		
		return platforms;
	}
	
	private class TrieNode(string locationSegment)
	{
		public string LocationSegment { get; } = locationSegment;
		public List<string> Platforms { get; } = [];
		public Dictionary<string, TrieNode> Children { get; } = [];
	}
}
