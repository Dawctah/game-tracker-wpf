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
        public static void AddCompleteGame(ListBox listBox, Game game, ref GameData gameData)
        {
            listBox.Items.Add(game);
            gameData.CompletedGames.Add(game);
        }

        public static void AddGame(Game game, ref GameData gameData)
        {
            gameData.GamesToPlay.Add(game);
            gameData.GamesToPlay.Sort((g1, g2) => g1.Priority.CompareTo(g2.Priority));
        }

        public static List<Game> Bump(List<Game> games, Game bumpedGame, bool up = true)
        {
            for (int k = 0; k < games.Count; k++)
            {
                if (games[k] == bumpedGame)
                {
                    if (k != 0 && k != games.Count - 1)
                    {
                        if (up)
                            Swap(ref games, k, k - 1);
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

        public static string FindName(List<Game> search, string current, TextBox textBox)
        {
            foreach (Game g in search)
            {
                if (g.Name.ToLower().Contains(textBox.Text.ToLower()))
                    current += g.ToString() + "\n";
            }

            return current;
        }

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

        public static void RefreshGamesListBox(List<Game> games, ListBox listBox, TextBlock textBlock, Label toPlay, Label completed, ref GameData gameData)
        {
            listBox.Items.Clear();
            foreach (Game g in games)
                listBox.Items.Add(g);

            toPlay.Content = gameData.GamesToPlay.Count;
            completed.Content = gameData.CompletedGames.Count;

            UpdateCompleteGameButton(textBlock, gameData);
        }

        public static void RemoveGame(Game game, ListBox listBox, TextBlock textBlock, ref GameData gameData)
        {
            gameData.GamesToPlay.Remove(game);
            listBox.Items.Remove(game);
            UpdateCompleteGameButton(textBlock, gameData);
        }

        public static void SaveData(string path, GameData gameData)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (Stream stream = File.Create(path))
            {
                formatter.Serialize(stream, gameData);
            }
        }

        public static void Swap(ref List<Game> games, int index1, int index2)
        {
            Game g1 = games[index1];
            Game g2 = games[index2];

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
