using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Commands
{
    public class BumpCommand : ICommand<IEnumerable<Game>>
    {
        private readonly IEnumerable<Game> games;
        private readonly Game bumpedGame;
        private readonly bool up;

        public BumpCommand(IEnumerable<Game> games, Game bumpedGame, bool up)
        {
            this.games = games;
            this.bumpedGame = bumpedGame;
            this.up = up;
        }

        public IEnumerable<Game> Execute()
        {
            var result = games.ToList();

            for (int k = 0; k < result.Count; k++)
            {
                if (result[k] == bumpedGame)
                {
                    if (k != 0 && k != result.Count - 1)
                    {
                        if (up) Swap(ref result, k, k - 1);
                        else
                            Swap(ref result, k, k + 1);
                    }
                    else if (k == 0 && !up)
                        Swap(ref result, k, k + 1);
                    else if (k == result.Count - 1 && up)
                        Swap(ref result, k, k - 1);

                    break;
                }
            }

            return result;
        }

        private static void Swap(ref List<Game> games, int index1, int index2)
        {
            Game g1 = games[index1];
            Game g2 = games[index2];

            (g2.Index, g1.Index) = (g1.Index, g2.Index);
            if (g1.Priority > g2.Priority)
            {
                g1.Priority--;
                g2.Priority++;
            }
            else if (g1.Priority < g2.Priority)
            {
                g1.Priority++;
                g2.Priority--;
            }

            games[index1] = g2;
            games[index2] = g1;
        }
    }
}
