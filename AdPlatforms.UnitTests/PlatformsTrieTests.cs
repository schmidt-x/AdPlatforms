using AdPlatforms.Common.Structures;

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
	public void ShouldReturnExpectedPlatforms(string[] searchLocation, string[] expectedPlatforms)
	{
		// Arrange

		// Яндекс.Директ:/ru 
		// Ревдинский рабочий:/ru/svrd/revda,/ru/svrd/pervik 
		// Газета уральских москвичей:/ru/msk,/ru/permobl,/ru/chelobl 
		// Крутая реклама:/ru/svrd 
		
		(string Platform, string[] LocationSegments)[] input = [
			("Яндекс.Директ",              [ "ru" ]),
			("Ревдинский рабочий",         [ "ru", "svrd", "revda" ]),
			("Ревдинский рабочий",         [ "ru", "svrd", "pervik" ]),
			("Газета уральских москвичей", [ "ru", "msk" ]),
			("Газета уральских москвичей", [ "ru", "permobl" ]),
			("Газета уральских москвичей", [ "ru", "chelobl" ]),
			("Крутая реклама",             [ "ru", "svrd" ]),
		];
		
		var trie = new PlatformTrie();
		
		foreach (var item in input)
		{
			trie.Add(item.Platform, item.LocationSegments);
		}
		
		// Act
		
		var actualPlatforms = trie.Find(searchLocation);
		
		// Assert
		
		Assert.Equal(expectedPlatforms.Length, actualPlatforms.Count);
		Assert.Equal(expectedPlatforms, actualPlatforms);
	}
	
	// TODO: test edge cases
}
