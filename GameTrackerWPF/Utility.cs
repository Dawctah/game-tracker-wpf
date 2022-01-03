using GameInformation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GameTrackerWPF
{
    public static class Utility
    {
        /// <summary>
        /// Add a game to the list of completed games.
        /// </summary>
        /// <param name="listBox">The list box to update.</param>
        /// <param name="game">The game to add.</param>
        /// <param name="gameData">The game data to alter.</param>
        public static void AddCompleteGame(ListBox listBox, Game game, ref GameData gameData)
        {
            listBox.Items.Add(game);
            gameData.CompletedGames.Add(game);
        }

        /// <summary>
        /// Add a game to the To-Play list.
        /// </summary>
        /// <param name="game">The game to add.</param>
        /// <param name="gameData">The game data to alter.</param>
        public static void AddGame(Game game, ref GameData gameData)
        {
            gameData.GamesToPlay.Add(game);
            gameData.GamesToPlay = Sort(gameData.GamesToPlay);
        }

        /// <summary>
        /// Bump a game up or down and adjust priority as needed.
        /// </summary>
        /// <param name="games">The list of games to reorder.</param>
        /// <param name="bumpedGame">The game that was bumped.</param>
        /// <param name="up">Which direction to bump.</param>
        /// <returns>The reordered list of games.</returns>
        public static List<Game> Bump(List<Game> games, Game bumpedGame, bool up = true)
        {
            for (int k = 0; k < games.Count; k++)
            {
                if (games[k] == bumpedGame)
                {
                    if (k != 0 && k != games.Count - 1)
                    {
                        if (up)Swap(ref games, k, k - 1);
                        else
                            Swap(ref games, k, k + 1);
                    }
                    else if (k == 0 && !up)
                        Swap(ref games, k, k + 1);
                    else if (k == games.Count - 1 && up)
                        Swap(ref games, k, k - 1);

                    break;
                }
            }

            return games;
        }

        /// <summary>
        /// Find games in a list to see if it is there.
        /// </summary>
        /// <param name="games">The list to search.</param>
        /// <param name="current">The current string.</param>
        /// <param name="search">The string to check.</param>
        /// <returns>The games that were found.</returns>
        public static string FindName(List<Game> games, string current, string search)
        {
            // Loop through all games to find all games with the desired search.
            foreach (Game g in games)
            {
                if (g.Name.ToLower().Contains(search.ToLower()))
                    current += g.ToString() + "\n";
            }

            return current;
        }

        /// <summary>
        /// Load the application data.
        /// </summary>
        /// <param name="path">The path to locate the save data from.</param>
        /// <returns>The loaded data.</returns>
        public static GameData LoadData(string path)
        {
            GameData result;

            BinaryFormatter f = new BinaryFormatter();
            FileStream fs;

            fs = new FileStream(path, FileMode.Open);
            result = (GameData)f.Deserialize(fs);
            fs.Close();

            return result;
        }

        /// <summary>
        /// Remove a game from a list.
        /// </summary>
        /// <param name="game">The game to remove.</param>
        /// <param name="listBox">The list box to update.</param>
        /// <param name="textBlock">The text block to update for the Complete Game button.</param>
        /// <param name="gameData">A reference to the game data.</param>
        public static void RemoveGame(Game game, ListBox listBox, TextBlock textBlock, ref GameData gameData)
        {
            gameData.GamesToPlay.Remove(game);
            listBox.Items.Remove(game);

            for (int k = 0; k < gameData.GamesToPlay.Count; k++)
            {
                gameData.GamesToPlay[k].Index = k + 1;
            }

            UpdateCompleteGameButton(textBlock, gameData);
        }

        /// <summary>
        /// Save the application information.
        /// </summary>
        /// <param name="path">The path to save the data in.</param>
        /// <param name="gameData">The game data to serialize.</param>
        public static void SaveData(string path, GameData gameData)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (Stream stream = File.Create(path))
            {
                formatter.Serialize(stream, gameData);
            }
        }

        public static List<Game> Sort(List<Game> games)
        {
            // Sort by index, since the new index should be set.
            games.Sort((g1, g2) => g1.Index.CompareTo(g2.Index));

            // Reset the game's indexes to remove any potential doubles.
            for (int k = 0; k < games.Count; k++)
                games[k].Index = k + 1;

            return games;
        }

        /// <summary>
        /// Swap two game's places in a list.
        /// </summary>
        /// <param name="games">The list to reorder.</param>
        /// <param name="index1">The first game's location.</param>
        /// <param name="index2">The second game's location.</param>
        public static void Swap(ref List<Game> games, int index1, int index2)
        {
            Game g1 = games[index1];
            Game g2 = games[index2];

            int game1Index = g1.Index;
            g1.Index = g2.Index;
            g2.Index = game1Index;

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

        /// <summary>
        /// Update the complete button text.
        /// </summary>
        /// <param name="textBlock">The text to update.</param>
        /// <param name="gameData">The game data to get the game from.</param>
        public static void UpdateCompleteGameButton(TextBlock textBlock, GameData gameData)
        {
            try
            {
                textBlock.Text = "Complete\n" + gameData.GamesToPlay[0].Name;
            }
            catch
            {
                textBlock.Text = "No Game to Complete";
            }
        }
    }
}
