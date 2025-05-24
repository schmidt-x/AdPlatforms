using AdPlatforms.Common.Structures;
using AdPlatforms.Domain.Models;

namespace AdPlatforms.UnitTests;

public class PlatformsTrieTests
{
	[Theory]
	[InlineData(new[] { "ru" },                   new[] { "Яндекс.Директ" })]
	[InlineData(new[] { "ru", "svrd" },           new[] { "Яндекс.Директ", "Крутая реклама" })]
	[InlineData(new[] { "ru", "msk" },            new[] { "Яндекс.Директ", "Газета уральских москвичей" })]
	[InlineData(new[] { "ru", "permobl" },        new[] { "Яндекс.Директ", "Газета уральских москвичей" })]
	[InlineData(new[] { "ru", "chelobl" },        new[] { "Яндекс.Директ", "Газета уральских москвичей" })]
	[InlineData(new[] { "ru", "svrd", "revda" },  new[] { "Яндекс.Директ", "Крутая реклама", "Ревдинский рабочий" })]
	[InlineData(new[] { "ru", "svrd", "pervik" }, new[] { "Яндекс.Директ", "Крутая реклама", "Ревдинский рабочий" })]
	public void ShouldReturnExpectedPlatforms(string[] locationSegments, string[] expectedPlatforms)
	{
		// Arrange

		// Яндекс.Директ:/ru 
		// Ревдинский рабочий:/ru/svrd/revda,/ru/svrd/pervik 
		// Газета уральских москвичей:/ru/msk,/ru/permobl,/ru/chelobl 
		// Крутая реклама:/ru/svrd 
		
		Platform[] platforms = [
			new("Яндекс.Директ", new Location("ru")),
			new("Ревдинский рабочий", new Location("ru", "svrd", "revda"), new Location("ru", "svrd", "pervik")),
			new("Газета уральских москвичей", new Location("ru", "msk"), new Location("ru", "permobl"), new Location("ru", "chelobl")),
			new("Крутая реклама", new Location("ru", "svrd"))
		];
		
		var trie = new PlatformTrie();
		
		foreach (var platform in platforms)
		{
			trie.Add(platform);
		}
		
		// Act
		
		var actualPlatforms = trie.Find(new Location(locationSegments));
		
		// Assert
		
		Assert.Equal(expectedPlatforms.Length, actualPlatforms.Count);
		Assert.Equal(expectedPlatforms, actualPlatforms);
	}
	
	// TODO: test edge cases
}
