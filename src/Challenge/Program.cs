// See https://aka.ms/new-console-template for more information

using Challenge.Configuration;
using Challenge.Configuration.Extensions;
using Challenge.Models;

if(!$"{args[0]}.json".TryToDeserialize<BoardSettings>(out var settings))
{
    Console.WriteLine("Invalid settings file. Press any key to exit.");
    return;
}

var game = new Game(settings);

if ($"{args[1]}.json".TryToDeserialize<GameSequences>(out var gameSequences))
{
    game.PlaySequences(gameSequences.Sequences);
    Console.WriteLine("End of game. Press any key to exit.");
}
else
{
    Console.WriteLine("Invalid moves file. Press any key to exit.");
}

Console.ReadLine();