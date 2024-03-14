namespace Challenge.Models;

using System.Text.Json.Serialization;

[method: JsonConstructor]
public struct Position(int x, int y)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;
}