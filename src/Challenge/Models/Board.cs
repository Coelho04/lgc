namespace Challenge.Models;

using Ardalis.GuardClauses;

using Challenge.Constants;
using Challenge.Enums;

public sealed class Board
{
    private readonly Turtle _turtle;
    private readonly IEnumerable<Position> _mines;
    private readonly Position _exitPosition;
private readonly BoardSize _size;
    public Board(BoardSize size, List<Position> mines, Position exitPosition, Position startingPosition, Direction startingDirection)
    {
        Guard.Against.Null(size, nameof(size), "Board size is required.");
        Guard.Against.Null(mines, nameof(mines), "Mines are required.");
        
        if (startingPosition.Equals(exitPosition))
        {
            throw new ArgumentException("Starting position cannot be the same as the exit position.");
        }
        
        if (mines.Any(minePosition => minePosition.Equals(startingPosition)))
        {
            throw new ArgumentException("Starting position cannot be in the same position as a mine.");
        }

        if (startingPosition.X < 0
            || startingPosition.Y < 0
            || startingPosition.X >= size.Width
            || startingPosition.Y >= size.Height)
        {
            throw new ArgumentException("Starting position cannot be is out of bounds.");
        }
        
        this._turtle = new Turtle(startingPosition, startingDirection);
        this._mines = mines;
        this._exitPosition = exitPosition;
        this._size = size;
    }
    
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
            || this._turtle.CurrentPosition.X >= this._size.Width
            || this._turtle.CurrentPosition.Y >= this._size.Height)
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
        if (this._exitPosition.Equals(this._turtle.CurrentPosition))
        {
            this._turtle.Status = Status.Escaped;
        }
    }
    
    public Status GetTurtleStatus() => this._turtle.Status;
    
    public Position GetTurtlePosition() => this._turtle.CurrentPosition;
    
    public Direction GetTurtleDirection() => this._turtle.CurrentDirection;
    
    public void ResetTurtle() => this._turtle.Reset();
}