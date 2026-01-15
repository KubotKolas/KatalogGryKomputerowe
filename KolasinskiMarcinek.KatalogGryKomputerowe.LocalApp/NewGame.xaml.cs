using KolasinskiMarcinek.KatalogGryKomputerowe.CORE;
using KolasinskiMarcinek.KatalogGryKomputerowe.INTERFACES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KolasinskiMarcinek.KatalogGryKomputerowe.LocalApp
{
    /// <summary>
    /// Logika interakcji dla klasy NewGame.xaml
    /// </summary>
    public partial class NewGame : Window
    {
        public NewGame(IEnumerable<string> producers)
        {
            InitializeComponent();
            SetDropDowns(producers);
        }

        public NewGame(IEnumerable<string> producers, IGame game)
        {
            InitializeComponent();
            SetDropDowns(producers);

            gameName.Text = game.Name;
            gameGenre.SelectedItem = GameGenreTranslator.Translate(game.Genre);
            gameProducer.SelectedItem = game.Producer.Name;
            ReleaseDate.Text = game.ReleaseYear.ToString();
            if(game.Multiplayer)
            {
                Multiplayer.Text = "Tak";
            }
            else
            {
                Multiplayer.Text = "Nie";
            }
        }

        private void SetDropDowns(IEnumerable<string> producers)
        {
            gameProducer.ItemsSource = producers.ToList();
            if (gameProducer.Items.Count > 0) gameProducer.SelectedIndex = 0;

            gameGenre.ItemsSource = gameGenre.ItemsSource = Enum.GetValues(typeof(GameGenre))
                .Cast<GameGenre>()
                .Select(g => GameGenreTranslator.Translate(g))
                .ToList();
            if (gameGenre.Items.Count > 0) gameGenre.SelectedIndex = 0;
        }

        public string GameName => gameName.Text;

        public GameGenre GameGenreEnum
        {
            get
            {
                return GameGenreTranslator.GetGenreByTranslation(gameGenre.Text); ;
            }
        }

        public string GameGenre => gameGenre.Text;

        public string GameProducer => gameProducer.Text;

        public int GameReleaseDate => int.TryParse(ReleaseDate.Text, out int year) ? year : 2026;

        private void Confirm(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(gameName.Text))
            {
                MessageBox.Show("Podaj nazwę gry.");
                return;
            }

            if (int.TryParse(ReleaseDate.Text, out int year))
            {
                if( year <= 1952 || year > 2026)
                {
                    MessageBox.Show("Podaj datę z między 1952 a 2026.");
                    return;
                }
            }

            DialogResult = true;
            Close();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            gameName.SelectAll();
            gameName.Focus();
        }

        private void ValidateReleaseYear(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);
        }

        private void Multiplayer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
