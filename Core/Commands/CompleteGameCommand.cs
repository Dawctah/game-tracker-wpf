using Core.Models;

namespace Core.Commands
{
    public class CompleteGameCommand : ICommand<GameData>
    {
        private readonly GameData gameData;

        public CompleteGameCommand(GameData gameData)
        {
            this.gameData = gameData;
        }

        public GameData Execute()
        {
            var completedGame = gameData.GamesToPlay.First();
            completedGame.DateCompleted = DateTime.Now;

            // Place game into completed list.
            var completedGames = gameData.CompletedGames.ToList();
            completedGames.Add(completedGame);

            foreach (var game in completedGames)
            {
                if (game.Priority != 0)
                {
                    game.Priority = 0;
                }
            }

            return gameData with
            {
                CompletedGames = completedGames
            };
        }
    }
}
