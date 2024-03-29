﻿namespace Challenge.UnitTests.Models
{
    using Challenge.Constants;
    using Challenge.Enums;
    using Challenge.Models;

    public class BoardTests
    {
        [Fact]
        public void PerformOperation_Move_OutOfBounds_TurtleStatusIsOutOfBounds()
        {
            // Arrange
            var size = new BoardSize { Height = 5, Width = 5 };
            var exitPosition = new Position(4, 4);
            var startingPosition = new Position(4, 3); // Near the boundary
            const Direction startingDirection = Direction.East;
            var board = new Board(size, [], exitPosition, startingPosition, startingDirection);

            // Act
            board.PerformOperation(Operations.Move);

            // Assert
            Assert.Equal(Status.OutOfBounds, board.GetTurtleStatus());
        }

        [Fact]
        public void PerformOperation_Move_HitMine_TurtleStatusIsDead()
        {
            // Arrange
            var size = new BoardSize { Height = 5, Width = 5 };
            var mines = new List<Position> { new(1, 0) }; // Mine at (1, 0)
            var exitPosition = new Position(4, 4);
            var startingPosition = new Position(0, 0);
            const Direction startingDirection = Direction.East;
            var board = new Board(size, mines, exitPosition, startingPosition, startingDirection);

            // Act
            board.PerformOperation(Operations.Move); // Move to (1, 0) where mine is present

            // Assert
            Assert.Equal(Status.Dead, board.GetTurtleStatus());
        }

        [Fact]
        public void PerformOperation_Move_Escape_TurtleStatusIsEscaped()
        {
            // Arrange
            var size = new BoardSize { Height = 5, Width = 5 };
            var exitPosition = new Position(1, 0); // Exit at (1, 0)
            var startingPosition = new Position(0, 0);
            const Direction startingDirection = Direction.East;
            var board = new Board(size, [], exitPosition, startingPosition, startingDirection);

            // Act
            board.PerformOperation(Operations.Move); // Move to the exit position

            // Assert
            Assert.Equal(Status.Escaped, board.GetTurtleStatus());
        }

        [Fact]
        public void PerformOperation_Rotate_TurtleRotates()
        {
            // Arrange
            var size = new BoardSize { Height = 5, Width = 5 };
            var exitPosition = new Position(4, 4);
            var startingPosition = new Position(0, 0);
            const Direction startingDirection = Direction.East;
            var board = new Board(size, [], exitPosition, startingPosition, startingDirection);

            // Act
            board.PerformOperation(Operations.Rotate);

            // Assert
            Assert.Equal(Direction.South, board.GetTurtleDirection());
        }

        [Fact]
        public void ResetTurtle_AfterMoving_TurtleResets()
        {
            // Arrange
            var size = new BoardSize { Height = 5, Width = 5 };
            var exitPosition = new Position(4, 4);
            var startingPosition = new Position(0, 0);
            const Direction startingDirection = Direction.East;
            var board = new Board(size, [], exitPosition, startingPosition, startingDirection);
            board.PerformOperation(Operations.Move); // Move turtle

            // Act
            board.ResetTurtle(); // Reset turtle

            // Assert
            Assert.Equal(startingPosition, board.GetTurtlePosition());
            Assert.Equal(startingDirection, board.GetTurtleDirection());
            Assert.Equal(Status.Alive, board.GetTurtleStatus());
        }
    }
}
