using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GameInformation;

namespace GameTrackerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameData gameData = new GameData();

        private readonly string directory = "C:\\Users\\" + WindowsIdentity.GetCurrent().Name.Split('\\')[1] + "\\AppData\\Roaming\\GameTracker\\";
        private const string fileName = "games.play";

        private string FilePath => directory + fileName;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddGameButton_Click(object sender, RoutedEventArgs e)
        {
            Game game = new Game();

            int sliderValue = 1;

            try
            {
                for (int k = 0; k < gameData.GamesToPlay.Count; k++)
                {
                    if (gameData.GamesToPlay[k + 1].Priority != gameData.GamesToPlay[k].Priority + 1)
                    {
                        sliderValue = gameData.GamesToPlay[k].Priority + 1;
                        break;
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                sliderValue = gameData.GamesToPlay[gameData.GamesToPlay.Count - 1].Priority + 1;
            }

            if (sliderValue > 10)
                sliderValue = 10;

            AddGame addGame = new AddGame(game, gameData, sliderValue);

            addGame.ShowDialog();

            if (addGame.DialogResult == true)
            {
                Utility.AddGame(game, ref gameData);

                Utility.RefreshGamesListBox(gameData.GamesToPlay, GamesListBox, CompleteTextBlock, ref gameData);
                Utility.UpdateCompleteGameButton(CompleteTextBlock, gameData);
            }
        }

        private void AutoSetPrio_Click(object sender, RoutedEventArgs e)
        {
            // Automatically set priorities to count up by 1.
            for (int k = 0; k < gameData.GamesToPlay.Count; k++)
            {
                gameData.GamesToPlay[k].Priority = k + 1;
            }

            Utility.RefreshGamesListBox(gameData.GamesToPlay, GamesListBox, CompleteTextBlock, ref gameData);
        }

        private void ClearDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(string.Format("Delete save data (this cannot be recovered?"), "Remove Game?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                File.Delete(FilePath);
                gameData = new GameData();
                Utility.RefreshGamesListBox(gameData.GamesToPlay, GamesListBox, CompleteTextBlock, ref gameData);
                Utility.RefreshGamesListBox(gameData.CompletedGames, CompletedListBox, CompleteTextBlock, ref gameData);
            }
        }

        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Add the game to a completed games list.
                Game completedGame = GamesListBox.Items[0] as Game;
                completedGame.Priority = 0;

                Utility.AddCompleteGame(CompletedListBox, completedGame, ref gameData);

                // Remove the game from the games list.
                Utility.RemoveGame(completedGame, GamesListBox, CompleteTextBlock, ref gameData);

                if (gameData.GamesToPlay.Count != 0)
                {
                    gameData.GamesToPlay[0].Priority = 1;

                    // Adjust the priority of all following games.
                    for (int k = 1; k < gameData.GamesToPlay.Count; k++)
                    {
                        // If the priorities are not in order, don't adjust the priority.
                        if (gameData.GamesToPlay[k].Priority == gameData.GamesToPlay[k - 1].Priority + 2)
                            gameData.GamesToPlay[k].Priority--;
                    }
                }

                Utility.RefreshGamesListBox(gameData.GamesToPlay, GamesListBox, CompleteTextBlock, ref gameData);
            }
            catch
            {
                MessageBox.Show("There is no game to complete.", "Error");
            }
        }

        private void CreateBackupButton_Click(object sender, RoutedEventArgs e)
        {
            bool failed = true;
            int backupsMade = 1;
            string backup = "games-backup-";
            string fullName = string.Empty;

            while (failed)
            {
                fullName = backup + backupsMade + ".play";
                if (!File.Exists(directory + fullName))
                {
                    Directory.CreateDirectory(directory);

                    Utility.SaveData(directory + fullName, gameData);
                    failed = false;
                }
                else
                    backupsMade++;
            }

            BackupFeedbackLabel.Text = fullName + " created";
        }

        private void GamesListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Game selection = GamesListBox.SelectedItem as Game;
            int placement = GamesListBox.Items.IndexOf(selection);

            AddGame addGame = new AddGame(selection, gameData, selection.Priority);

            addGame.ShowDialog();

            if (addGame.DialogResult == true)
            {
                // Update the game.
                GamesListBox.Items[placement] = selection;

                gameData.GamesToPlay.Sort((g1, g2) => g1.Priority.CompareTo(g2.Priority));
                Utility.RefreshGamesListBox(gameData.GamesToPlay, GamesListBox, CompleteTextBlock, ref gameData);
            }
        }

        private void GamesSearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            GamesSearchBox.Text = string.Empty;
        }

        private void GamesSearchBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            string result = "Games to Play:\n";
            result = Utility.FindName(gameData.GamesToPlay, result, GamesSearchBox);

            result += "\nCompleted Games:\n";
            result = Utility.FindName(gameData.CompletedGames, result, GamesSearchBox);

            MessageBox.Show(result, "Results for " + GamesSearchBox.Text + ":");

            Keyboard.Focus(AddGameButton);
        }

        private void GamesSearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            GamesSearchBox.Text = "Search games...";
        }

        private void RemoveGameButton_Click(object sender, RoutedEventArgs e)
        {
            Game selection = GamesListBox.SelectedItem as Game;

            if (selection != null)
            {
                if (MessageBox.Show($"Are you sure you want to remove {selection.Name}?", "Remove Game?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    Utility.RemoveGame(selection, GamesListBox, CompleteTextBlock, ref gameData);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (gameData.CompletedGames.Count != 0 || gameData.GamesToPlay.Count != 0)
            {
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                Utility.SaveData(FilePath, gameData);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                gameData = Utility.LoadData(FilePath);
            }
            catch
            {
                MessageBox.Show("Previously saved game data not loaded. You may not have data to load.", "Error Loading Data");
            }

            Utility.RefreshGamesListBox(gameData.GamesToPlay, GamesListBox, CompleteTextBlock, ref gameData);
            Utility.RefreshGamesListBox(gameData.CompletedGames, CompletedListBox, CompleteTextBlock, ref gameData);
        }

        private void OpenBackupsFolder_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            Process.Start(directory);
        }
    }
}
