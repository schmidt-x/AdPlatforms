using System.Collections.Generic;
using System.Diagnostics;

namespace AdPlatforms.Common.Structures;

internal class PlatformTrie
{
	private readonly TrieNode _root = new(string.Empty);
	
	internal void Add(string platform, IReadOnlyCollection<string> locationSegments)
	{
		Debug.Assert(locationSegments.Count > 0, "Root node cannot have Platforms.");
		
		var current = _root;
		
		foreach (var segment in locationSegments)
		{
			if (current.Children.Count == 0 || !current.Children.TryGetValue(segment, out var child))
			{
				child = new TrieNode(segment);
				current.Children.Add(segment, child);
			}
			
			current = child;
		}
		
		current.Platforms.Add(platform); // Here 'current' is the last node.
	}
	
	internal IReadOnlyCollection<string> Find(IReadOnlyCollection<string> locationSegments)
	{
		List<string> platforms = [];
		var current = _root;
		
		foreach (var segment in locationSegments)
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
