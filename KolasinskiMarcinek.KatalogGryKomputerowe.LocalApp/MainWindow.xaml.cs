using KolasinskiMarcinek.KatalogGryKomputerowe.BL;
using KolasinskiMarcinek.KatalogGryKomputerowe.CORE;
using KolasinskiMarcinek.KatalogGryKomputerowe.INTERFACES;
using KolasinskiMarcinek.KatalogGryKomputerowe.LocalApp.ViewModel;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KolasinskiMarcinek.KatalogGryKomputerowe.LocalApp;

public partial class MainWindow : Window
{
    public ViewModel.GamesListViewModel gamesListViewModel { get; } = new ViewModel.GamesListViewModel();
    private ViewModel.GamesViewModel selectedGame = null;

    public ViewModel.ProducerListViewModel producerListViewModel { get; } = new ViewModel.ProducerListViewModel();
    private ViewModel.ProducerViewModel selectedProducer = null;

    private BusinessLogic businessLogic;

    public MainWindow()
    {
        IConfiguration config = new ConfigurationBuilder().Build();

        businessLogic = BusinessLogic.GetInstance(config);

        InitializeComponent();
        Refresh();
    }

    private void Refresh()
    {
        producerListViewModel.RefreshList(businessLogic.GetAllProducers());
        gamesListViewModel.RefreshList(businessLogic.GetAllGames());
        LoadAdresses();
    }

    private void LoadAdresses()
    {
        ProducerFilterValueComboBox.ItemsSource = null;

        var addresses = businessLogic.GetAllProducers()
            .Select(p => p.Address)
            .Where(addr => !string.IsNullOrEmpty(addr))
            .Distinct()
            .OrderBy(addr => addr)
            .ToList();

        ProducerFilterValueComboBox.ItemsSource = addresses;
    }

    private void FilterTypeComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (filterValueComboBox == null) return;
        string selectedValue = (filterTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

        switch (selectedValue)
        {
            case "Wieloosobowy":
                filterValueComboBox.ItemsSource = new List<string> { "Tak", "Nie" };
                break;
            case "Rok Wydania":
                filterValueComboBox.ItemsSource = businessLogic.GetAllGames()
                    .Select(g => g.ReleaseYear.ToString())         
                    .Distinct()                          
                    .OrderByDescending(year => year)    
                    .ToList();
                break;
            case "Twórca":
                filterValueComboBox.ItemsSource = businessLogic.GetAllProducers()
                    .Select(p => p.Name)
                    .Distinct()
                    .OrderBy(name => name)
                    .ToList();
                break;
            case "Gatunek":
                filterValueComboBox.ItemsSource = GameGenreTranslator.GetTranslatedValues();
                break;
            default:
                filterValueComboBox.ItemsSource = null;
                break;
        }
    }

    private void FilterValueComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (filterValueComboBox.SelectedItem == null || filterTypeComboBox.SelectedItem == null)
        {
            return;
        }
        GameApplyFilter();
    }

    private void GameApplyFilter()
    {
        var selectedType = (filterTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
        var filterValue = filterValueComboBox.SelectedItem as string;

        if (string.IsNullOrEmpty(filterValue))
        {
            gamesListViewModel.RefreshList(businessLogic.GetAllGames());
            return;
        }

        switch (selectedType)
        {
            case "Wieloosobowy":
                bool isMultiplayer = true;
                if (filterValue == "Nie")
                {
                    isMultiplayer = false;
                }
                gamesListViewModel.RefreshList(businessLogic.GetAllGames().Where(g => g.Multiplayer == isMultiplayer));
                break;
            case "Rok Wydania":
                gamesListViewModel.RefreshList(businessLogic.GetAllGames().Where(g => g.ReleaseYear == int.Parse(filterValue)));
                break;
            case "Twórca":
                gamesListViewModel.RefreshList(businessLogic.GetAllGames().Where(g => g.Producer.Name == filterValue));
                break;
            case "Gatunek":
                GameGenre genre = GameGenreTranslator.GetGenreByTranslation(filterValue);
                gamesListViewModel.RefreshList(businessLogic.GetAllGames().Where(g => g.Genre == genre));
                break;
        }
    }
    
    private void EnterApplyGameSearch(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            ApplyGameSearch(sender, e);

            Keyboard.ClearFocus();
        }
    }

    private void EnterApplyProducerSearch(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            ApplyProducerSearch(sender, e);

            Keyboard.ClearFocus();
        }
    }

    

    private void ApplyGameSearch(object sender, RoutedEventArgs e)
    {
        gamesListViewModel.RefreshList(businessLogic.GetAllGames().Where(g => g.Name.Contains(gameSearchField.Text, StringComparison.OrdinalIgnoreCase)));
    }

    private void RemoveFiltersGame(object sender, RoutedEventArgs e)
    {
        gameSearchField.Clear();
        filterValueComboBox.SelectedItem = null;
        gamesListViewModel.RefreshList(businessLogic.GetAllGames());
    }

    private void ProducerFilterTypeComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ProducerFilterValueComboBox.SelectedItem == null || ProducerFilterValueComboBox.SelectedItem == null)
        {
            return;
        }
        ProducerApplyFilter();
    }

    private void ProducerApplyFilter()
    {
        string address = ProducerFilterValueComboBox.SelectedItem as string;
        producerListViewModel.RefreshList(string.IsNullOrEmpty(address)
            ? businessLogic.GetAllProducers()
            : businessLogic.GetAllProducers().Where(p => p.Address.Equals(address)));
    }

    private void ApplyProducerSearch(object sender, RoutedEventArgs e)
    {
        producerListViewModel.RefreshList(businessLogic.GetAllProducers().Where(p => p.Name.Contains(producerSearchField.Text,StringComparison.OrdinalIgnoreCase)));
    }

    private void RemoveFiltersProducer(object sender, RoutedEventArgs e)
    {
        producerSearchField.Clear();
        ProducerFilterValueComboBox.SelectedItem = null;
        producerListViewModel.RefreshList(businessLogic.GetAllProducers());
    }

    private void GameListSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        selectedGame = GamesList.SelectedItem as GamesViewModel;
    }
    private void AddGame(object sender, RoutedEventArgs e)
    {

        IEnumerable<string> producers = businessLogic.GetAllProducers().Select(p => p.Name);

        NewGame dialog = new NewGame(producers);

        bool? result = dialog.ShowDialog();

        try
        {
            var producerObj = businessLogic.GetAllProducers().First(p => p.Name == dialog.GameProducer);

            IGame newGame = new Game()
            {
                Name = dialog.GameName,
                Genre = dialog.GameGenreEnum,
                Producer = producerObj,
                ReleaseYear = dialog.GameReleaseDate,
            };

            businessLogic.CreateNewGame(newGame);
            Refresh();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Wystąpił błąd podczas dodawania gry: " + ex.Message);
        }
    }

    private void EditGame(object sender, RoutedEventArgs e)
    {
        if (selectedGame == null) return;

        IGame currentGame = businessLogic.GetAllGames().Where(g => g.Id == selectedGame.GameId).First();
        IEnumerable<string> producers = businessLogic.GetAllProducers().Select(p => p.Name);

        NewGame dialog = new NewGame(producers, currentGame);

        if (dialog.ShowDialog() == true)
        {
            var producerObj = businessLogic.GetAllProducers().First(p => p.Name == dialog.GameProducer);

            currentGame.Name = dialog.GameName;
            currentGame.Genre = dialog.GameGenreEnum;
            currentGame.Producer = producerObj;
            currentGame.ReleaseYear = dialog.GameReleaseDate;

            businessLogic.UpdateGame(currentGame);
            Refresh();
        }
    }

    private void RemoveGame(object sender, RoutedEventArgs e)
    {
        if (selectedGame == null) return;
        if (MessageBox.Show($"Usunąć {selectedGame.GameName}?", "Akceptuj", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
            businessLogic.DeleteGame(selectedGame.GameId);
            Refresh();
            selectedGame = null;
        }
    }

    private void ProducerListSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        selectedProducer = ProducerList.SelectedItem as ProducerViewModel;
    }

    private void AddProducer(object sender, RoutedEventArgs e)
    {

        NewProducer dialog = new NewProducer();
        if (dialog.ShowDialog() == true)
        {
            IProducer producer = new Producer()
            {
                Name = dialog.ProducerName,
                Address = dialog.ProducerAddress
            };

            businessLogic.CreateNewProducer(producer);
            Refresh();
        }
    }

    private void EditProducer(object sender, RoutedEventArgs e)
    {
        if (selectedProducer == null) return;

        IProducer producer = businessLogic.GetAllProducers().Where(g => g.Id == selectedProducer.ProducerId).First();

        NewProducer dialog = new NewProducer(producer);

        if (dialog.ShowDialog() == true)
        {
            producer.Name = dialog.ProducerName;
            producer.Address = dialog.ProducerAddress;

            businessLogic.UpdateProducer(producer);
            Refresh();
        }
    }

    private void RemoveProducer(object sender, RoutedEventArgs e)
    {
        if (selectedProducer == null) return;
        if (MessageBox.Show("Usunąć twórcę gry?", "Akceptuj", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
            businessLogic.DeleteProducer(selectedProducer.ProducerId);
            Refresh();
            selectedProducer = null;
        }
    }
}