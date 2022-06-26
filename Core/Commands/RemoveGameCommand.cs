using Core.Models;

namespace Core.Commands
{
    public record RemoveGameCommandContext(IEnumerable<Game> Games, Game Game);
    public class RemoveGameCommand : ICommand<IEnumerable<Game>>
    {
        private readonly RemoveGameCommandContext context;

        public RemoveGameCommand(RemoveGameCommandContext context)
        {
            this.context = context;
        }

        public IEnumerable<Game> Execute()
        {
            var games = context.Games.ToList();

            if (games.Contains(context.Game))
            {
                games.Remove(context.Game);
            }

            return games;
        }
    }
}
