namespace Challenge.Models;

using Challenge.Enums;

public sealed class Turtle(Position initialPosition, Direction initialDirection)
{
    private readonly Position _initialPosition = initialPosition;
    private readonly Direction _initialDirection = initialDirection;
    private Position _currentPosition = initialPosition;

    public Position CurrentPosition => this._currentPosition;

    public Direction CurrentDirection { get; private set; } = initialDirection;

    public Status Status { get; set; }
    
    public void Rotate() =>
        this.CurrentDirection = this.CurrentDirection switch
        {
            Direction.North => Direction.East,
            Direction.East => Direction.South,
            Direction.South => Direction.West,
            Direction.West => Direction.North,
            _ => this.CurrentDirection
        };

    public void Move()
    {
        switch (this.CurrentDirection)
        {
            case Direction.North:
                this._currentPosition.Y -= 1;
                break;
            case Direction.East:
                this._currentPosition.X += 1;
                break;
            case Direction.South:
                this._currentPosition.Y += 1;
                break;
            case Direction.West:
                this._currentPosition.X -= 1;
                break;
            default:
                break;
        }
    }
    
    public void Reset()
    {
        this._currentPosition = this._initialPosition;
        this.CurrentDirection = this._initialDirection;

        this.Status = Status.Alive;
    }
}