using System.IO;
using AdPlatforms.Common;
using AdPlatforms.Domain.Enums;

namespace AdPlatforms.UnitTests;

public class HelpersTests
{
	[Fact]
	public void ShouldFailIfNoPlatformIsGiven()
	{
		// Arrange
		const string input = "\n\n\n";
		using var sr =  new StringReader(input);
		
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
		const string input = "Platform /location";
		using var sr =  new StringReader(input);
		
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
		const string input = ":/location";
		using var sr =  new StringReader(input);
		
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
		const string input = "Platform:";
		using var sr =  new StringReader(input);
		
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
		const string input = "Platform:,,,";
		using var sr =  new StringReader(input);
		
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
		using var sr =  new StringReader(input);
		
		// Act
		var result = Helpers.ParsePlatforms(sr);
		
		// Assert
		Assert.True(result.IsError(out var error, out _));
		Assert.Equal(ErrorCode.EmptyLocation, error.Code);
		Assert.Equal($"Location must contain at least one non-empty segment. Line: {line}.", error.Description);
	}
}