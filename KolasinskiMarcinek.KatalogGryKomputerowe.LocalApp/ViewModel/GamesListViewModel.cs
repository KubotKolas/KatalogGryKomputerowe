using KolasinskiMarcinek.KatalogGryKomputerowe.INTERFACES;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KolasinskiMarcinek.KatalogGryKomputerowe.LocalApp.ViewModel
{
    public class GamesListViewModel
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<GamesViewModel> gamesList { get; set; } = new ObservableCollection<GamesViewModel>();

        public void RefreshList(IEnumerable<IGame> games)
        {
            gamesList.Clear();

            foreach (var item in games)
            {
                gamesList.Add(new GamesViewModel(item));
            }

            RaisePropertyChanged(nameof(gamesList));
        }

        private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
