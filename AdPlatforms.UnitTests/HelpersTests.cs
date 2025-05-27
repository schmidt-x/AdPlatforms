using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdPlatforms.Common;
using AdPlatforms.Domain.Enums;
using AdPlatforms.Domain.Models;

namespace AdPlatforms.UnitTests;

public class HelpersTests
{
	[Fact]
	public void ShouldFailIfNoPlatformIsGiven()
	{
		// Arrange
		using var sr = new StringReader("\n\n\n");
		
		// Act
		var result = Helpers.ParsePlatforms(sr);
		
		// Assert
		Assert.True(result.IsError(out var error, out _));
		Assert.Equal(ErrorCode.EmptyPlatform, error.Code);
		Assert.Equal("At least one Platform is required.", error.Description);
	}
	
	[Fact]
	public void ShouldFailIfColonIsMissing()
	{
		// Arrange
		using var sr = new StringReader("Platform /location");
		
		// Act
		var result = Helpers.ParsePlatforms(sr);
		
		// Assert
		Assert.True(result.IsError(out var error, out _));
		Assert.Equal(ErrorCode.InvalidValue, error.Code);
		Assert.Equal("Platform and Location(s) must be separated by a colon ':'. Line: 1.", error.Description);
	}
	
	[Fact]
	public void ShouldFailIfPlatformIsEmpty()
	{
		// Arrange
		using var sr = new StringReader(":/location");
		
		// Act
		var result = Helpers.ParsePlatforms(sr);
		
		// Assert
		Assert.True(result.IsError(out var error, out _));
		Assert.Equal(ErrorCode.EmptyPlatform, error.Code);
		Assert.Equal("Platform name is required. Line: 1.", error.Description);
	}
	
	[Fact]
	public void ShouldFailIfLocationIsEmpty()
	{
		// Arrange
		using var sr = new StringReader("Platform:");
		
		// Act
		var result = Helpers.ParsePlatforms(sr);
		
		// Assert
		Assert.True(result.IsError(out var error, out _));
		Assert.Equal(ErrorCode.EmptyLocation, error.Code);
		Assert.Equal("At least one Location is required. Line: 1.", error.Description);
	}
	
	[Fact]
	public void ShouldFailIfLocationContainsNoElements()
	{
		// Arrange
		using var sr = new StringReader("Platform:,,,");
		
		// Act
		var result = Helpers.ParsePlatforms(sr);
		
		// Assert
		Assert.True(result.IsError(out var error, out _));
		Assert.Equal(ErrorCode.EmptyLocation, error.Code);
		Assert.Equal("At least one Location is required. Line: 1.", error.Description);
	}
	
	[Theory]
	[InlineData("Platform:/", 1)]
	[InlineData("Platform:/en\r\nPlatform2:/en,//", 2)]
	public void ShouldFailIfLocationElementConsistsOfEmptySegments(string input, int line)
	{
		// Arrange
		using var sr = new StringReader(input);
		
		// Act
		var result = Helpers.ParsePlatforms(sr);
		
		// Assert
		Assert.True(result.IsError(out var error, out _));
		Assert.Equal(ErrorCode.EmptyLocation, error.Code);
		Assert.Equal($"Location must contain at least one non-empty segment. Line: {line}.", error.Description);
	}
	
	[Fact]
	public void ShouldTrimWhitespaces()
	{
		// Arrange
		using var sr = new StringReader("  Platform   : /a  ,  / a / b / c  ");
		
		const string expectedPlatformName = "Platform";
		List<List<string>> expectedLocations = [ [ "a" ], [ "a", "b", "c" ] ];
		
		// Act
		var result = Helpers.ParsePlatforms(sr);
		
		// Assert
		
		Assert.True(result.IsOk(out var platforms, out _));
		Assert.Single(platforms);
		
		var platform = platforms.First();
		
		Assert.Equal(expectedPlatformName, platform.Name);
		Assert.Equal(expectedLocations.Count, platform.Locations.Count);
		
		var locations = (IList<Location>)platform.Locations;
		for (int i = 0; i < expectedLocations.Count; i++)
		{
			Assert.Equal(expectedLocations[i], locations[i].Segments);
		}
	}
}