using System.Collections.Generic;
using System.IO;
using AdPlatforms.Common;
using AdPlatforms.Common.Structures;
using AdPlatforms.Domain.Models;

namespace AdPlatforms.Services;

public class PlatformService : IPlatformService
{
	private static PlatformTrie _trie = new();
	
	public Error? Save(Stream stream)
	{
		using TextReader textReader = new StreamReader(stream);
		
		if (Helpers.ParsePlatforms(textReader).IsError(out var error, out var platforms))
		{
			return error;
		}
		
		var trie = new PlatformTrie();
		
		foreach (var platform in platforms)
		{
			trie.Add(platform);
		}
		
		_trie = trie;
		return null;
	}

	public Result<IReadOnlyCollection<string>, Error> Get(string location)
	{
		if (Helpers.ParseLocation(location).IsError(out var error, out var parsedLocation))
		{
			return error;
		}
		
		var platforms = _trie.Find(parsedLocation);
		return Result<IReadOnlyCollection<string>, Error>.Success(platforms);
	}
}