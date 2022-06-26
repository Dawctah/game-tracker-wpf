using Core.Models;

namespace Core.Commands
{
    public class SearchGameCommand : ICommand<IEnumerable<Game>>
    {
        private readonly IEnumerable<Game> games;
        private readonly string search;

        public SearchGameCommand(IEnumerable<Game> games, string search)
        {
            this.games = games;
            this.search = search.ToLower();
        }

        public IEnumerable<Game> Execute()
        {
            var result = new List<Game>();

            if (!string.IsNullOrEmpty(search))
            {
                foreach (var game in games)
                {
                    if (game.Name.ToLower().Contains(search.ToLower()))
                    {
                        result.Add(game);
                    }
                }
            }

            return result;
        }
    }
}
