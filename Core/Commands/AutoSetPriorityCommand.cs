using Core.Models;

namespace Core.Commands
{
    public class AutoSetPriorityCommand : ICommand<IEnumerable<Game>>
    {
        private readonly IEnumerable<Game> games;

        public AutoSetPriorityCommand(IEnumerable<Game> games)
        {
            this.games = games;
        }

        public IEnumerable<Game> Execute()
        {
            var result = games.ToList();
            for (int k = 0; k < result.Count; k++)
            {
                result[k].Priority = k + 1;
                result[k].Index = k + 1;
            }

            return result;
        }
    }
}
