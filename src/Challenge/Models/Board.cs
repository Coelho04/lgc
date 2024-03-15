namespace Challenge.Models;

using Challenge.Constants;
using Challenge.Enums;

public sealed class Board(
    BoardSize size,
    List<Position> mines,
    Position exitPosition,
    Position startingPosition,
    Direction startingDirection)
{
    private readonly Turtle _turtle = new(startingPosition, startingDirection);
    private readonly IEnumerable<Position> _mines = mines;

    public void PerformOperation(char operation)
    {
        switch (operation)
        {
            case Operations.Move:
                this._turtle.Move();
                break;
            case Operations.Rotate:
                this._turtle.Rotate();
                break;
        }

        this.UpdateTurtleStatus();
    }

    private void UpdateTurtleStatus()
    {
        // Out of bounds
        if (this._turtle.CurrentPosition.X < 0
            || this._turtle.CurrentPosition.Y < 0
            || this._turtle.CurrentPosition.X >= size.Width
            || this._turtle.CurrentPosition.Y >= size.Height)
        {
            this._turtle.Status = Status.OutOfBounds;
            return;
        }
        
        // Hit a mine
        if (this._mines.Any(minePosition => minePosition.Equals(this._turtle.CurrentPosition)))
        {
            this._turtle.Status = Status.Dead;
            return;
        }

        // Escaped
        if (exitPosition.Equals(this._turtle.CurrentPosition))
        {
            this._turtle.Status = Status.Escaped;
        }
    }
    
    public Status GetTurtleStatus() => this._turtle.Status;
    
    public Position GetTurtlePosition() => this._turtle.CurrentPosition;
    
    public Direction GetTurtleDirection() => this._turtle.CurrentDirection;
    
    public void ResetTurtle() => this._turtle.Reset();
}