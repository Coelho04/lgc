namespace Challenge.UnitTests.Models;

using Challenge.Configuration;
using Challenge.Constants;
using Challenge.Enums;
using Challenge.Models;

public class GameTests
{
    [Fact]
    public void PlaySequences_TurtleOutOfBounds_OutputSequenceAndStop()
    {
        // Arrange
        var settings = new BoardSettings
        {
            Size = new BoardSize { Height = 5, Width = 5 },
            Mines = new List<Position>(),
            ExitPosition = new Position(4, 4),
            StartingPosition = new Position(0, 0),
            StartingDirection = Direction.East
        };
        var game = new Game(settings);
        var sequences = new List<Moves> { new() { Operations.Move, Operations.Move, Operations.Move, Operations.Move, Operations.Move } };
        
        // Act
        using var consoleOutput = new ConsoleOutput();
        game.PlaySequences(sequences);

        // Assert
        var output = consoleOutput.GetOutput();
        Assert.Contains("Sequence 0: Turtle has moved off the board!", output);
        Assert.DoesNotContain("Sequence 0: Success!", output);
    }

    [Fact]
    public void PlaySequences_TurtleHitsMine_OutputSequenceAndStop()
    {
        // Arrange
        var settings = new BoardSettings
        {
            Size = new BoardSize { Height = 5, Width = 5 },
            Mines = new List<Position> { new(1, 1) }, // Place a mine at (1, 1)
            ExitPosition = new Position(4, 4),
            StartingPosition = new Position(0, 0),
            StartingDirection = Direction.East
        };
        var game = new Game(settings);

        var sequences = new List<Moves> { new() { Operations.Move, Operations.Rotate, Operations.Move } };
        
        // Act
        using var consoleOutput = new ConsoleOutput();
        game.PlaySequences(sequences);

        // Assert
        var output = consoleOutput.GetOutput();
        Assert.Contains("Sequence 0: Mine Hit!", output);
        Assert.DoesNotContain("Sequence 0: Success!", output);
    }

    [Fact]
    public void PlaySequences_TurtleEscapes_OutputSequenceAndStop()
    {
        // Arrange
        var settings = new BoardSettings
        {
            Size = new BoardSize { Height = 5, Width = 5 },
            Mines = new List<Position>(),
            ExitPosition = new Position(1, 0), // Set exit position at (1, 0)
            StartingPosition = new Position(0, 0),
            StartingDirection = Direction.East
        };
        var game = new Game(settings);
        var sequences = new List<Moves> { new() { Operations.Move } };

        // Act
        using var consoleOutput = new ConsoleOutput();
        game.PlaySequences(sequences);

        // Assert
        var output = consoleOutput.GetOutput();
        Assert.Contains("Sequence 0: Success!", output);
        Assert.DoesNotContain("Sequence 0: Still in danger!", output);
    }

    [Fact]
    public void PlaySequences_TurtleStillAlive_OutputSequenceAndContinue()
    {
        // Arrange
        var settings = new BoardSettings
        {
            Size = new BoardSize { Height = 5, Width = 5 },
            Mines = new List<Position>(),
            ExitPosition = new Position(4, 4),
            StartingPosition = new Position(0, 0),
            StartingDirection = Direction.East
        };
        var game = new Game(settings);
        var sequences = new List<Moves> { new() { Operations.Move, Operations.Move, Operations.Move } };
        
        // Act
        using var consoleOutput = new ConsoleOutput();
        game.PlaySequences(sequences);

        // Assert
        var output = consoleOutput.GetOutput();
        Assert.Contains("Sequence 0: Still in danger!", output);
    }
    
    private class ConsoleOutput : IDisposable
    {
        private readonly StringWriter _stringWriter;
        private readonly TextWriter _originalOutput;

        public ConsoleOutput()
        {
            this._stringWriter = new StringWriter();
            this._originalOutput = Console.Out;
            Console.SetOut(this._stringWriter);
        }

        public string GetOutput() => this._stringWriter.ToString();

        public void Dispose()
        {
            Console.SetOut(this._originalOutput);
            this._stringWriter.Dispose();
        }
    }
}