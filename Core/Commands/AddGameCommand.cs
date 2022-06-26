using Core.Models;

namespace Core.Commands
{
    public record AddGameCommandContext(List<Game> Games, Game Game);
    public class AddGameCommand : ICommand<IEnumerable<Game>>
    {
        private readonly AddGameCommandContext context;
        public AddGameCommand(AddGameCommandContext context)
        {
            this.context = context;
        }

        public IEnumerable<Game> Execute()
        {
            if (context.Game == null) return context.Games;
            if (string.IsNullOrEmpty(context.Game.Name)) return context.Games;

            var games = context.Games;
            games.Add(context.Game);

            return games;
        }
    }
}
