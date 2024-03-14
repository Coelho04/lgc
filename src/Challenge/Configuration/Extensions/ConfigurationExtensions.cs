namespace Challenge.Configuration.Extensions;

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

public static class ConfigurationExtensions
{
    private static readonly JsonSerializerOptions JsonOptions =
        new(JsonSerializerDefaults.Web) { Converters = { new JsonStringEnumConverter() } };

    public static bool TryToDeserialize<T>(this string? file, [NotNullWhen(true)]out T? result)
    {
        if (string.IsNullOrWhiteSpace(file))
        {
            Console.Write("File path cannot be null or empty");
            result = default;
            return false;
        }

        try
        {
            using var r = new StreamReader(file);
            var json = r.ReadToEnd();
            result = JsonSerializer.Deserialize<T>(json, JsonOptions);
        }
        catch
        {
            result = default;
        }
        
        if (result is not null)
        {
            return true;
        }

        Console.WriteLine($"File {file} could not be deserialized to {typeof(T).Name}");
        return false;
    }
}