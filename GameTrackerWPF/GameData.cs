using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameInformation;

namespace GameTrackerWPF
{
    [Serializable]
    public class GameData
    {
        public List<Game> GamesToPlay { get; set; }
        public List<Game> CompletedGames { get; set; }

        public GameData()
        {
            GamesToPlay = new List<Game>();
            CompletedGames = new List<Game>();
        }
    }
}
