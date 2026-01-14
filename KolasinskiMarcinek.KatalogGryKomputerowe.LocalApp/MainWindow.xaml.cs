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
        this.DataContext = this;
        IConfiguration config = new ConfigurationBuilder().Build();

        businessLogic = BusinessLogic.GetInstance(config);

        InitializeComponent();
        Refresh();
    }

    private void Refresh()
    {
        // Refresh ViewModels from Business Logic
        producerListViewModel.RefreshList(businessLogic.GetAllProducers());
        gamesListViewModel.RefreshList(businessLogic.GetAllGames());

        var allGames = businessLogic.GetAllGames();

        // Message Box z listą gier z Business Logic
        string gameListBL = "Gry z Business Logic:\n\n";
        int i = 1;
        foreach (var game in allGames)
        {
            gameListBL += $"{i}. ID: {game.Id}, Nazwa: {game.Name}, Rok: {game.ReleaseYear}, Producent: {game.Producer?.Name ?? "Brak"}\n";
            i++;
        }
        MessageBox.Show(gameListBL, $"Business Logic - Liczba gier: {allGames.Count()}");

        // Message Box z listą gier z ViewModel
        string gameListVM = "Gry w ViewModel (gamesListViewModel.gamesList):\n\n";
        int j = 1;
        foreach (var gameVM in gamesListViewModel.gamesList)
        {
            gameListVM += $"{j}. ID: {gameVM.GameId}, Nazwa: {gameVM.GameName}, Rok: {gameVM.GameReleaseYear}, Producent: {gameVM.GameProducerName}\n";
            j++;
        }
        MessageBox.Show(gameListVM, $"ViewModel - Liczba gier: {gamesListViewModel.gamesList.Count}");

    }

    private void FilterTypeComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (filterValueComboBox == null) return;
        string selectedValue = (filterTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

        switch (selectedValue)
        {
            case "Wieloosobowy":
                filterValueComboBox.ItemsSource = businessLogic.GetAllGames()
                    .Select(g => g.Multiplayer)
                    .Distinct()
                    .OrderByDescending(m => m)
                    .Select(m => m ? "Tak" : "Nie")
                    .ToList();
                break;
            case "Data Wydania":
                filterValueComboBox.ItemsSource = businessLogic.GetAllGames()
                    .Select(g => g.ReleaseYear)         
                    .Distinct()                          
                    .OrderByDescending(year => year)    
                    .ToList();
                break;
            case "Twórca":
                filterValueComboBox.ItemsSource = businessLogic.GetAllProducers();
                break;
            case "Gatunek":
                filterValueComboBox.ItemsSource = GameGenreTranslator.GetTranslatedValues();
                break;
            default:
                filterValueComboBox.ItemsSource = null;
                break;
        }
    }

    private void GameApplyFilter(object sender, RoutedEventArgs e)
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
                //Enum.TryParse(filterValue, out CPUSocketType socket);
                bool isMultiplayer = true;
                if (filterValue == "Nie")
                {
                    isMultiplayer = false;
                }
                gamesListViewModel.RefreshList(businessLogic.GetAllGames().Where(g => g.Multiplayer == isMultiplayer));
                break;
            case "Data Wydania":
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

    private void ProducerApplyFilter(object sender, RoutedEventArgs e)
    {
        string address = ProducerFilterValueComboBox.SelectedItem as string;
        producerListViewModel.RefreshList(string.IsNullOrEmpty(address)
            ? businessLogic.GetAllProducers()
            : businessLogic.GetAllProducers().Where(p => p.Address.Equals(address)));
    }

    private void ApplyProducerSearch(object sender, RoutedEventArgs e)
    {
        producerListViewModel.RefreshList(businessLogic.GetAllProducers().Where(p => p.Name.Equals(producerSearchField.Text)));
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
        var producer = businessLogic.GetAllProducers();
        /*
        NewCPU dialog = new NewCPU(manufacturers);

        if (dialog.ShowDialog() == true)
        {
            try
            {
                var manufacturerObj = _bl.GetAllManufacturers().First(m => m.Name == dialog.SelectedManufacturer);

                ICPU newCpu = new CPU()
                {
                    Name = dialog.CPUName,
                    Cores = dialog.Cores,
                    Threads = dialog.Threads,
                    BaseClockGHz = dialog.BaseClock,
                    SocketType = dialog.CPUSocket,
                    manufacturer = manufacturerObj
                };

                _bl.CreateCPU(newCpu);
                RefreshAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding CPU: " + ex.Message);
            }
        }*/
    }

    private void EditGame(object sender, RoutedEventArgs e)
    {
        if (selectedGame == null) return;

        IGame currentCpu = businessLogic.GetAllGames().Where(g => g.Id == selectedGame.GameId).First();
        var producer = businessLogic.GetAllProducers();
        
        //TODO DOKOŃCZYĆ 2 OKNO

        /*NewCPU dialog = new NewCPU(manufacturers, currentCpu);

        if (dialog.ShowDialog() == true)
        {
            var manufacturerObj = _bl.GetAllManufacturers().First(m => m.Name == dialog.SelectedManufacturer);

            currentCpu.Name = dialog.CPUName;
            currentCpu.Cores = dialog.Cores;
            currentCpu.Threads = dialog.Threads;
            currentCpu.BaseClockGHz = dialog.BaseClock;
            currentCpu.SocketType = dialog.CPUSocket;
            currentCpu.manufacturer = manufacturerObj;

            _bl.UpdateCPU(currentCpu);
            RefreshAll();
        }*/
    }

    private void RemoveGame(object sender, RoutedEventArgs e)
    {
        if (selectedGame == null) return;
        if (MessageBox.Show($"Remove {selectedGame.GameName}?", "Akceptuj", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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
        //TODO DOKOŃCZYĆ 3 OKNO

        /*NewManufacturer dialog = new NewManufacturer();
        if (dialog.ShowDialog() == true)
        {
            IManufacturer manufacturer = new Manufacturer()
            {
                Name = dialog.ManufacturerName,
                Address = dialog.ManufacturerAddress
            };

            _bl.CreateManufacturer(manufacturer);
            RefreshAll();
        }*/
    }

    private void EditProducer(object sender, RoutedEventArgs e)
    {
        if (selectedProducer == null) return;

        IProducer current = businessLogic.GetAllProducers().Where(g => g.Id == selectedProducer.ProdcuerId).First();

        //TODO DOKOŃCZYĆ 3 OKNO

        /*NewManufacturer dialog = new NewManufacturer(current);

        if (dialog.ShowDialog() == true)
        {
            current.Name = dialog.ManufacturerName;
            current.Address = dialog.ManufacturerAddress;

            _bl.UpdateManufacturer(current);
            RefreshAll();
        }*/
    }

    private void RemoveProducer(object sender, RoutedEventArgs e)
    {
        if (selectedProducer == null) return;
        if (MessageBox.Show("Remove manufacturer?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
            businessLogic.DeleteProducer(selectedProducer.ProdcuerId);
            Refresh();
            selectedProducer = null;
        }
    }


    private IEnumerable<string> GetMultiplayerOptions()
    {
        yield return "Tak";
        yield return "Nie";
    }

}