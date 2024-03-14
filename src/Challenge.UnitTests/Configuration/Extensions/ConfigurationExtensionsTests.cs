namespace Challenge.UnitTests.Configuration.Extensions;

using Challenge.Configuration.Extensions;

public class ConfigurationExtensionsTests
{
    [Fact]
    public void TryToDeserialize_ValidJsonFile_ReturnsTrueAndDeserializedObject()
    {
        // Arrange
        var filePath = Path.Combine(AppContext.BaseDirectory, "valid.json");
        const string jsonContent = "{\"Name\":\"John\",\"Age\":30}";
        File.WriteAllText(filePath, jsonContent);

        // Act
        var result = filePath.TryToDeserialize<Person>(out var person);

        // Assert
        Assert.True(result);
        Assert.NotNull(person);
        Assert.Equal("John", person.Name);
        Assert.Equal(30, person.Age);

        // Clean-up
        File.Delete(filePath);
    }

    [Fact]
    public void TryToDeserialize_InvalidJsonFile_ReturnsFalseAndNullObject()
    {
        // Arrange
        var filePath = Path.Combine(AppContext.BaseDirectory, "invalid.json");
        const string jsonContent = "{invalid_json}";
        File.WriteAllText(filePath, jsonContent);

        // Act
        var result = filePath.TryToDeserialize<Person>(out var person);

        // Assert
        Assert.False(result);
        Assert.Null(person);

        // Clean-up
        File.Delete(filePath);
    }

    [Fact]
    public void TryToDeserialize_NullOrEmptyFilePath_ReturnsFalseAndNullObject()
    {
        // Act
        var result1 = ((string?)null).TryToDeserialize<Person>(out var person1);
        var result2 = "".TryToDeserialize<Person>(out var person2);

        // Assert
        Assert.False(result1);
        Assert.Null(person1);
        Assert.False(result2);
        Assert.Null(person2);
    }

    // Define a sample class for deserialization
    private class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}