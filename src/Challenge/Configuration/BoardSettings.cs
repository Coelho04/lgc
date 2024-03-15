namespace Challenge.Configuration;

using Challenge.Enums;
using Challenge.Models;

public class BoardSettings
{
    public BoardSize Size { get; set; } = new();

    public Direction StartingDirection { get; set; }
    
    public Position StartingPosition { get; set; }
    
    public Position ExitPosition { get; set; }
    
    public List<Position> Mines { get; set; } = [];
}