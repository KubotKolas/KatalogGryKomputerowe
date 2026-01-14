using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KolasinskiMarcinek.KatalogGryKomputerowe.BL;
using KolasinskiMarcinek.KatalogGryKomputerowe.CORE;
using KolasinskiMarcinek.KatalogGryKomputerowe.INTERFACES;

namespace KolasinskiMarcinek.KatalogGryKomputerowe.LocalApp.ViewModel
{
    public class GamesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private IGame game;

        public GamesViewModel(IGame game)
        {
            this.game = game;
        }

        public int GameId
        {
            get => game.Id;
            set
            {
                game.Id = value;
                RaisePropertyChanged(nameof(GameId));
            }
        }

        public string GameName
        {
            get => game.Name;
            set
            {
                game.Name = value;
                RaisePropertyChanged(nameof(GameName));
            }
        }

        public string GameProducerName
        {
            get => game.Producer.Name;
            set
            {
                game.Producer.Name = value;
                RaisePropertyChanged(nameof(GameProducerName));
            }
        }

        public bool GameMultiplayer
        {
            get => game.Multiplayer;
            set
            {
                game.Multiplayer = value;
                RaisePropertyChanged(nameof(GameMultiplayer));
            }
        }

        public int GameReleaseYear
        {
            get => game.ReleaseYear;
            set
            {
                game.ReleaseYear = value;
                RaisePropertyChanged(nameof(GameReleaseYear));
            }
        }

        public GameGenre GameGenre
        {
            get => game.Genre;
            set
            {
                game.Genre = value;
                RaisePropertyChanged(nameof(GameGenre));
            }
        }

        private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
