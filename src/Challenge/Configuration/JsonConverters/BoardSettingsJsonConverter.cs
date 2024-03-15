namespace Challenge.Configuration.JsonConverters;

using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

using Challenge.Models;

public class BoardSettingsJsonConverter : JsonConverter<BoardSettings>
{
    private static readonly JsonSerializerOptions DefaultJsonSerializerOptions = new(JsonSerializerDefaults.Web) { Converters = { new JsonStringEnumConverter() } };
    
    public override BoardSettings? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        
        var node = JsonNode.Parse(ref reader);

        if (node is not JsonObject)
        {
            return null;
        }

        var boardSettings = node.Deserialize<BoardSettings>(DefaultJsonSerializerOptions);

        if (boardSettings is null)
        {
            return null;
        }
        
        // Validate the deserialized object
        if (boardSettings.Size.Height <= 0 || boardSettings.Size.Width <= 0)
        {
            throw new JsonException($"Invalid board size, Height: {boardSettings.Size.Height}, Width {boardSettings.Size.Width}");
        }
        
        if (boardSettings.StartingPosition.Equals(boardSettings.ExitPosition))
        {
            throw new JsonException("Starting position cannot be the same as the exit position.");
        }
        
        if (boardSettings.Mines.Any(minePosition => minePosition.Equals(boardSettings.StartingPosition)))
        {
            throw new JsonException("Starting position cannot be in the same position as a mine.");
        }

        if (boardSettings.StartingPosition.X < 0
            || boardSettings.StartingPosition.Y < 0
            || boardSettings.StartingPosition.X >= boardSettings.Size.Width
            || boardSettings.StartingPosition.Y >= boardSettings.Size.Height)
        {
            throw new JsonException("Starting position cannot be is out of bounds.");
        }

        return boardSettings;
    }

    public override void Write(Utf8JsonWriter writer, BoardSettings value, JsonSerializerOptions _) => JsonSerializer.Serialize(writer, value, DefaultJsonSerializerOptions);
}