namespace Challenge.Models;

using Challenge.Configuration;
using Challenge.Enums;

public class Game(BoardSettings settings)
{
    private readonly Board _board = new(settings.Size, settings.Mines, settings.ExitPosition, settings.StartingPosition, settings.StartingDirection);
    
    public void PlaySequences(List<Moves> sequences)
    {
        for(var i = 0; i < sequences.Count; i++)
        {
            foreach (var move in sequences[i])
            {
                this._board.PerformOperation(move);

                if (!ShouldContinue(this._board.GetTurtleStatus(), i))
                {
                    break;
                }
            }

            if (this._board.GetTurtleStatus() == Status.Alive)
            {
                Console.WriteLine($"Sequence {i}: Still in danger!");
            }
            
            this._board.ResetTurtle();
            
            Console.WriteLine();
        }
    }

    private static bool ShouldContinue(Status status, int sequence)
    {
        switch (status)
        {
            case Status.OutOfBounds:
                Console.WriteLine($"Sequence {sequence}: Turtle has moved off the board!");
                return false;
            case Status.Dead:
                Console.WriteLine($"Sequence {sequence}: Mine Hit!");
                return false;
            case Status.Escaped:
                Console.WriteLine($"Sequence {sequence}: Success!");
                return false;
            case Status.Alive:
            default:
                return true;
        }
    }
}