using Newtonsoft.Json;

namespace Core.Models
{
    [JsonObject]
    public record GameData (IEnumerable<Game> GamesToPlay, IEnumerable<Game> CompletedGames)
    {
        public static GameData Empty = new GameData(Enumerable.Empty<Game>(), Enumerable.Empty<Game>());
    }
}
