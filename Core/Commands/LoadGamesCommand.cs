using Core.Models;
using Newtonsoft.Json;

namespace Core.Commands
{
    public record LoadGamesCommandContext(string Path);
    public class LoadGamesCommand : ICommand<GameData>
    {
        private readonly LoadGamesCommandContext context;

        public LoadGamesCommand(LoadGamesCommandContext context)
        {
            this.context = context;
        }

        public GameData Execute()
        {
            try
            {
                var data = File.ReadAllText(context.Path);
                var result = JsonConvert.DeserializeObject<GameData>(data);
                return result!;
            }
            catch (FileNotFoundException)
            {
                return GameData.Empty;
            }
        }
    }
}
