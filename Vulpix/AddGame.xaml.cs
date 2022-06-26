using Core.Models;
using System.Windows;

namespace Vulpix
{
    /// <summary>
    /// Interaction logic for AddGame.xaml
    /// </summary>
    public partial class AddGame : Window
    {
        private readonly bool updating;
        private readonly int defaultSliderValue;

        private readonly Game game;

        public AddGame(Game game, int defaultSliderValue, bool updating = false)
        {
            InitializeComponent();
            this.game = game;
            this.updating = updating;

            if (defaultSliderValue > 0 && defaultSliderValue < 21)
            {
                this.defaultSliderValue = defaultSliderValue;
            }
            else
            {
                this.defaultSliderValue = 1;
            }
        }

        private void AddGameButton_Click(object sender, RoutedEventArgs e)
        {
            game.Name = GameNameTextBox.Text;
            game.Priority = (int)PrioritySlider.Value;
            game.Owned = (bool)Owned.IsChecked!;
            if (Hours.Text != string.Empty)
            {
                try
                {
                    game.HoursToBeatAvg = float.Parse(Hours.Text);
                }
                catch
                {
                    game.HoursToBeatAvg = 0;
                }
            }

            DialogResult = true;
        }

        private void PrioritySlider_ValueChanged(object sender, RoutedEventArgs e)
        {
            PrioritySliderValue.Content = (int)PrioritySlider.Value;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PrioritySlider.Minimum = 1;
            PrioritySlider.Maximum = 20;

            PrioritySlider.Value = defaultSliderValue;

            GameNameTextBox.Text = game.Name;
            Owned.IsChecked = game.Owned;

            if (updating)
            {
                Title = "Vulpix - Update Game";
                AddGameButton.Content = "Update Game";
            }

            GameNameTextBox.Focus();
        }
    }
}
