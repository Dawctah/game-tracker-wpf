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
