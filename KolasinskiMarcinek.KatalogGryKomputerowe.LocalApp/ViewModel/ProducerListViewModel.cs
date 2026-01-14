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
    public class ProducerListViewModel
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<ProducerViewModel> producerList { get; set; } = new ObservableCollection<ProducerViewModel>();

        public void RefreshList(IEnumerable<IProducer> producer)
        {
            producerList.Clear();

            foreach (var item in producer)
            {
                producerList.Add(new ProducerViewModel(item));
            }

            RaisePropertyChanged(nameof(producerList));
        }

        private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
