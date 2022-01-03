using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GameInformation;

namespace GameTrackerWPF
{
    /// <summary>
    /// Interaction logic for AddGame.xaml
    /// </summary>
    public partial class AddGame : Window
    {
        private Game game;
        private GameData gameData;
        private int defaultSliderVal;
        private bool priorityChanged = false;

        private bool Updating => AddGameButton.Content.ToString().Contains("Update");

        public AddGame(Game game, GameData gameData, int defaultSliderVal)
        {
            this.game = game;
            this.gameData = gameData;
            this.defaultSliderVal = defaultSliderVal;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PrioritySlider.Maximum = 10;
            PrioritySlider.Minimum = 1;

            PrioritySlider.Value = defaultSliderVal;
            PrioritySliderValue.Content = (int)PrioritySlider.Value;

            GameNameTextBox.Text = game.Name;

            Owned.IsChecked = game.Owned;

            if (game.Name != null)
                AddGameButton.Content = "Update Game";

            GameNameTextBox.Focus();
        }

        private void PrioritySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            PrioritySliderValue.Content = (int)PrioritySlider.Value;

            if (Updating)
                priorityChanged = true;
        }

        private void AddGameButton_Click(object sender, RoutedEventArgs e)
        {
            // Search the lists in game data to see if a game with that name already exists.
            if (!(AddGameButton.Content as string).Contains("Update") && gameData.GamesToPlay.Find(x => x.Name.ToLower() == GameNameTextBox.Text.ToLower()) != null)
            {
                if (MessageBox.Show($"A game called {GameNameTextBox.Text} is already in your to-play list. Add it anyway?", "Add Duplicate Game?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    Complete();
            }
            else if (!(AddGameButton.Content as string).Contains("Update") && gameData.CompletedGames.Find(x => x.Name.ToLower() == GameNameTextBox.Text.ToLower()) != null)
            {
                if (MessageBox.Show($"You already beat a game called {GameNameTextBox.Text}. Add it to your play-list anyway?", "Add Duplicate Game?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    Complete();
            }
            else
                Complete();
        }

        private void Complete()
        {
            game.Name = GameNameTextBox.Text;
            game.Priority = (int)PrioritySlider.Value;
            if (priorityChanged)
            {
                if (game.Priority < game.Index)
                    game.Index = game.Priority - 1;
                else
                    game.Index = game.Priority + 1;
            }
            else if (game.Priority != 10)
                game.Index = game.Priority;

            game.Owned = (bool)Owned.IsChecked;

            DialogResult = true;
        }
    }
}
