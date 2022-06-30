using Core.Commands;
using Core.Data;
using Core.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Vulpix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // private readonly string directory = "C:\\Users\\" + WindowsIdentity.GetCurrent().Name.Split('\\')[1] + "\\AppData\\Roaming\\GameTracker\\";
        // private readonly string fileName = "games.ninetales";

        // private string FilePath => directory + fileName;

        private GameData data;
        private GameData GameData
        {
            get => data;
            set
            {
                data = value with
                {
                    GamesToPlay = value.GamesToPlay.OrderBy(x => x.Priority).ToList()
                };

                RefreshGamesListBox();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            data = GameData.Empty;
        }

        private void AddGameButton_Click(object sender, RoutedEventArgs e)
        {
            var game = new Game();

            var sliderValue = 1;
            var list = GameData.GamesToPlay.ToList();

            try
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i + 1].Priority != list[i].Priority + 1)
                    {
                        sliderValue = list[i].Priority + 1;
                        break;
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                sliderValue = list[^1].Priority + 1;
            }

            if (sliderValue > 20)
            {
                sliderValue = 20;
            }

            var addGameWindow = new AddGame(game, sliderValue);
            addGameWindow.ShowDialog();

            if (addGameWindow.DialogResult == true)
            {
                var addGameCommand = new AddGameCommand(new(GameData.GamesToPlay.ToList(), game));
                var games = addGameCommand.Execute();
                GameData = GameData with
                {
                    GamesToPlay = games
                };
            }
        }

        private void AutoSetPrio_Click(object sender, RoutedEventArgs e)
        {
            var result = new AutoSetPriorityCommand(GameData.GamesToPlay).Execute();

            GameData = GameData with
            {
                GamesToPlay = result
            };
        }

        private void BumpDownButton_Click(object sender, RoutedEventArgs e) => Bump(false);

        private void BumpUpButton_Click(object sender, RoutedEventArgs e) => Bump();

        private void ClearDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(string.Format("Delete save data (this cannot be recovered?"), "Clear Data?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                File.Delete(FileNames.NewFullPath);
                GameData = GameData.Empty;
            }
        }

        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (GameData.GamesToPlay.Any())
            {
                var result = new CompleteGameCommand(GameData).Execute();
                var games = new RemoveGameCommand(new(result.GamesToPlay, result.GamesToPlay.First())).Execute();
                games = new AutoSetPriorityCommand(games).Execute();

                GameData = result with
                {
                    GamesToPlay = games
                };
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
                fullName = backup + backupsMade + ".ninetales";
                if (!File.Exists(FileNames.Directory + fullName))
                {
                    Directory.CreateDirectory(FileNames.Directory);

                    new SaveGamesCommand(new(GameData, FileNames.Directory, fullName)).Execute();
                    failed = false;
                }
                else
                {
                    backupsMade++;
                }
            }

            BackupFeedbackLabel.Text = fullName + " created";
        }

        private void GamesListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (GamesListBox.SelectedItem is Game selection)
            {
                var addGame = new AddGame(selection, selection.Priority, true);

                addGame.ShowDialog();

                if (addGame.DialogResult == true)
                {
#pragma warning disable CA2245 // Do not assign a property to itself. Refreshing priority.
                    GameData = GameData;
#pragma warning restore CA2245 // Do not assign a property to itself

                    GamesListBox.SelectedItem = selection;
                }
            }
        }

        private void GamesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GamesListBox.SelectedItem is Game selectedGame)
            {
                BumpUpButton.Visibility = Visibility.Visible;
                BumpDownButton.Visibility = Visibility.Visible;

                if (selectedGame.HoursToBeatAvg != 0)
                {
                    HowLongToBeat.Text = $"{selectedGame.HoursToBeatAvg} hours to beat.";
                }
                else
                {
                    HowLongToBeat.Text = string.Empty;
                }
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

            if (string.IsNullOrEmpty(GamesSearchBox.Text)) return;

            var result = "Games to Play:\n";
            var toPlay = new SearchGameCommand(GameData.GamesToPlay, GamesSearchBox.Text).Execute();
            var completed = new SearchGameCommand(GameData.CompletedGames, GamesSearchBox.Text).Execute();

            foreach (var game in toPlay)
            {
                result += $"{game}\n";
            }

            result += "Completed Games:\n";
            foreach (var game in completed)
            {
                result += $"{game}\n";
            }

            MessageBox.Show(result, $"Results for {GamesSearchBox.Text}:");
            Keyboard.Focus(AddGameButton);
        }

        private void GamesSearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            GamesSearchBox.Text = "Search games...";
        }

        private void OpenBackupsFolder_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(FileNames.Directory))
                Directory.CreateDirectory(FileNames.Directory);

            var t = Environment.GetEnvironmentVariable("WINDIR") + @"\explorer.exe";
            Process.Start(t, FileNames.Directory);
        }

        private void RemoveGameButton_Click(object sender, RoutedEventArgs e)
        {
            var selection = GamesListBox.SelectedItem as Game;
            if (selection != null)
            {
                if (MessageBox.Show($"Are you sure you want to remove {selection.Name}?", "Remove Game?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var removeCommand = new RemoveGameCommand(new(GameData.GamesToPlay, selection));
                    var games = removeCommand.Execute();

                    GameData = GameData with
                    {
                        GamesToPlay = games
                    };
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var saveCommand = new SaveGamesCommand(new(GameData, FileNames.Directory, FileNames.NewFileName));
            saveCommand.Execute();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Load all of the game data.
            var loadCommand = new LoadGamesCommand(new (FileNames.NewFullPath));

            GameData = loadCommand.Execute();
        }

        private void Bump(bool up = true)
        {
            if (GamesListBox.SelectedItem is Game selection)
            {
                var newList = new BumpCommand(GameData.GamesToPlay, selection, up).Execute();
                GameData = GameData with
                {
                    GamesToPlay = newList
                };

                GamesListBox.SelectedItem = selection;
            }
        }

        private void RefreshGamesListBox()
        {
            CompletedListBox.Items.Clear();
            GamesListBox.Items.Clear();

            foreach (var game in GameData.GamesToPlay)
            {
                GamesListBox.Items.Add(game);
            }

            foreach (var game in GameData.CompletedGames)
            {
                CompletedListBox.Items.Add(game);
            }

            ToPlayCount.Content = GameData.GamesToPlay.Count();
            CompletedCount.Content = GameData.CompletedGames.Count();

            if (GameData.GamesToPlay.Any())
            {
                CompleteButton.Content = $"Complete\n{GameData.GamesToPlay.First().Name}";
            }
            else
            {
                CompleteButton.Content = "No Game to Complete";
            }
        }
    }
}
