using Core.Commands;
using Core.Data;
using Core.Models;
using System.Runtime.Serialization.Formatters.Binary;

var gameData = GameData.Empty;
Console.Title = "Test Console - Intended for Developer Use Only";

var running = true;
var commands = new string[]
{
    "add",
    "load",
    "json",
    "exit"
};

while (running)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("This console program is intended for developer use only.");
    Console.ForegroundColor = ConsoleColor.DarkGreen;
    foreach (var command in commands)
    {
        Console.WriteLine(command);
    }

    Console.WriteLine("");
    var input = Console.ReadLine()!;
    if (input == "add")
    {
        var game = new Game("Soul Hackers 2", 1, 10, false, 0);

        var addCommand = new AddGameCommand(new(new List<Game>(), game));
        var games = (List<Game>)addCommand.Execute();
        gameData = gameData with
        {
            GamesToPlay = games
        };

        var saveCommand = new SaveGamesCommand(new (gameData, FileNames.Directory, FileNames.TestFileName));
        saveCommand.Execute();
    }
    else if (input == "load")
    {
        var loadCommand = new LoadGamesCommand(new(FileNames.TestFullPath));

        gameData = (GameData)loadCommand.Execute();

        Console.WriteLine(gameData.GamesToPlay.First().Name);
    }
    else if (input == "json")
    {
        var f = new BinaryFormatter();

        var fs = new FileStream(FileNames.Directory + FileNames.OldFileName, FileMode.Open);
#pragma warning disable SYSLIB0011 // Type or member is obsolete. Shut up :(
        var oldData = (GameTrackerWPF.GameData)f.Deserialize(fs);
#pragma warning restore SYSLIB0011 // Type or member is obsolete

        var gamesToPlay = new List<Game>();
        foreach (GameInformation.Game game in oldData.GamesToPlay)
        {
            gamesToPlay.Add(new Game(game.Name, game.Index, game.Priority, game.Owned, (float)game.HoursToBeatAvg));
        }

        var gamesCompleted = new List<Game>();
        foreach(GameInformation.Game game in oldData.CompletedGames)
        {
            gamesCompleted.Add(new Game(game.Name, game.Index, game.Priority, game.Owned, (float)game.HoursToBeatAvg));
        }

        var result = new GameData(gamesToPlay, gamesCompleted);
        new SaveGamesCommand(new(result, FileNames.Directory, FileNames.NewFileName)).Execute();

        fs.Close();

        Console.WriteLine("Data transferred.\n");
    }
    else if (input == "exit")
    {
        running = false;
    }
}