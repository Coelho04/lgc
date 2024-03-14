namespace Challenge.UnitTests.Models;

using Challenge.Enums;
using Challenge.Models;

public class TurtleTests
{
    [Fact]
    public void Rotate_WhenCalled_ChangesDirection()
    {
        // Arrange
        var turtle = new Turtle(new Position(0, 0), Direction.North);

        // Act
        turtle.Rotate(); // Rotate once

        // Assert
        Assert.Equal(Direction.East, turtle.CurrentDirection);

        // Rotate three more times to check full rotation
        turtle.Rotate();
        Assert.Equal(Direction.South, turtle.CurrentDirection);

        turtle.Rotate();
        Assert.Equal(Direction.West, turtle.CurrentDirection);

        turtle.Rotate();
        Assert.Equal(Direction.North, turtle.CurrentDirection);
    }

    [Fact]
    public void Move_WhenCalled_MovesTurtle()
    {
        // Arrange
        var turtle = new Turtle(new Position(0, 0), Direction.North);

        // Act
        turtle.Move(); // Move once

        // Assert
        Assert.Equal(0, turtle.CurrentPosition.X);
        Assert.Equal(-1, turtle.CurrentPosition.Y);

        // Move to the east and check position
        turtle.Rotate(); // Change direction to East
        turtle.Move();
        Assert.Equal(1, turtle.CurrentPosition.X);
        Assert.Equal(-1, turtle.CurrentPosition.Y);

        // Move to the south and check position
        turtle.Rotate(); // Change direction to South
        turtle.Move();
        Assert.Equal(1, turtle.CurrentPosition.X);
        Assert.Equal(0, turtle.CurrentPosition.Y);

        // Move to the west and check position
        turtle.Rotate(); // Change direction to West
        turtle.Move();
        Assert.Equal(0, turtle.CurrentPosition.X);
        Assert.Equal(0, turtle.CurrentPosition.Y);
    }

    [Fact]
    public void Reset_WhenCalled_ResetsTurtle()
    {
        // Arrange
        var initialPosition = new Position(3, 3);
        var initialDirection = Direction.South;
        var turtle = new Turtle(initialPosition, initialDirection);

        // Act
        turtle.Move(); // Move turtle from initial position
        turtle.Rotate(); // Change direction
        turtle.Reset(); // Reset turtle to initial state

        // Assert
        Assert.Equal(initialPosition, turtle.CurrentPosition);
        Assert.Equal(initialDirection, turtle.CurrentDirection);
    }
    
    [Fact]
    public void Rotate_FourTimes_NoDirectionChange()
    {
        // Arrange
        var turtle = new Turtle(new Position(0, 0), Direction.North);

        // Act
        turtle.Rotate();
        turtle.Rotate();
        turtle.Rotate();
        turtle.Rotate();

        // Assert
        Assert.Equal(Direction.North, turtle.CurrentDirection);
    }
}