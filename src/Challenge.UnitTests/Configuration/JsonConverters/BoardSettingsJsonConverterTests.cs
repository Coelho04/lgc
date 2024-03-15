namespace Challenge.UnitTests.Configuration.JsonConverters;

using System.Text;
using System.Text.Json;

using Challenge.Configuration;
using Challenge.Configuration.JsonConverters;
using Challenge.Enums;
using Challenge.Models;

public class BoardSettingsJsonConverterTests
{
    [Fact]
    public void Read_ValidJson_ReturnsBoardSettings()
    {
        // Arrange
        const string jsonString = """
                                  {
                                                  "Size": { "Height": 5, "Width": 5 },
                                                  "Mines": [],
                                                  "ExitPosition": { "X": 4, "Y": 4 },
                                                  "StartingPosition": { "X": 0, "Y": 0 },
                                                  "StartingDirection": "East"
                                              }
                                  """;
        var jsonBytes = Encoding.UTF8.GetBytes(jsonString);
        var reader = new Utf8JsonReader(jsonBytes);
        var converter = new BoardSettingsJsonConverter();
        var options = new JsonSerializerOptions();

        // Act
        var result = converter.Read(ref reader, typeof(BoardSettings), options);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Size.Height);
        Assert.Equal(5, result.Size.Width);
        Assert.Empty(result.Mines);
        Assert.Equal(new Position(4, 4), result.ExitPosition);
        Assert.Equal(new Position(0, 0), result.StartingPosition);
        Assert.Equal(Direction.East, result.StartingDirection);
    }

    [Theory]
    [InlineData("{ }", "Invalid board size, Height: 0, Width 0")]
    [InlineData("{ \"Size\": {\"Height\": 0, \"Width\": 5} }", "Invalid board size, Height: 0, Width 5")]
    [InlineData("{ \"Size\": {\"Height\": 5, \"Width\": 0} }", "Invalid board size, Height: 5, Width 0")]
    [InlineData("{ \"Size\": {\"Height\": -1, \"Width\": 5} }", "Invalid board size, Height: -1, Width 5")]
    [InlineData("{ \"Size\": {\"Height\": 5, \"Width\": -1} }", "Invalid board size, Height: 5, Width -1")]
    [InlineData("{ \"Size\": {\"Height\": 5, \"Width\": 5}, \"StartingPosition\": {\"X\": 5, \"Y\": 5} }", "Starting position cannot be is out of bounds.")]
    [InlineData("{ \"Size\": {\"Height\": 5, \"Width\": 5}, \"StartingPosition\": {\"X\": 0, \"Y\": 0}, \"ExitPosition\": {\"X\": 0, \"Y\": 0} }", "Starting position cannot be the same as the exit position.")]
    [InlineData("{ \"Size\": {\"Height\": 5, \"Width\": 5}, \"StartingPosition\": {\"X\": 0, \"Y\": 0}, \"Mines\": [{\"X\": 0, \"Y\": 0}], \"ExitPosition\": {\"X\": 4, \"Y\": 4} }", "Starting position cannot be in the same position as a mine.")]
    public void Read_InvalidJson_ThrowsJsonException(string jsonString, string expectedErrorMessage)
    {
        // Arrange
        var jsonBytes = Encoding.UTF8.GetBytes(jsonString);
        var reader = new Utf8JsonReader(jsonBytes);
        var converter = new BoardSettingsJsonConverter();
        var options = new JsonSerializerOptions();

        // Act & Assert
        
        Exception? exception = null;
        
        try
        {
            _ = converter.Read(ref reader, typeof(BoardSettings), options);
        }
        catch (Exception e)
        {
            exception = e;
        }
        
        Assert.NotNull(exception);
        Assert.Equal(expectedErrorMessage, exception.Message);
    }

    [Fact]
    public void Write_ValidBoardSettings_ReturnsJsonString()
    {
        // Arrange
        var settings = new BoardSettings
        {
            Size = new BoardSize{Height = 5, Width = 5},
            Mines = [],
            ExitPosition = new Position(4, 4),
            StartingPosition = new Position(0, 0),
            StartingDirection = Direction.East
        };
        
        var expectedJsonString = "{\"size\":{\"height\":5,\"width\":5},\"startingDirection\":\"East\",\"startingPosition\":{\"x\":0,\"y\":0},\"exitPosition\":{\"x\":4,\"y\":4},\"mines\":[]}";

        // Act
        var converter = new BoardSettingsJsonConverter();
        using var memoryStream = new MemoryStream();
        using var writer = new Utf8JsonWriter(memoryStream);
        converter.Write(writer, settings, new JsonSerializerOptions());
        writer.Flush();
        var jsonBytes = memoryStream.ToArray();
        var actualJsonString = Encoding.UTF8.GetString(jsonBytes);

        // Assert
        Assert.Equal(expectedJsonString, actualJsonString);
    }
}