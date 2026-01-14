using KolasinskiMarcinek.KatalogGryKomputerowe.INTERFACES;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KolasinskiMarcinek.KatalogGryKomputerowe.LocalApp.ViewModel
{
    public class ProducerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private IProducer producer;

        public ProducerViewModel(IProducer producer)
        {
            this.producer = producer;
        }

        public int ProdcuerId
        {
            get => producer.Id;
            set
            {
                producer.Id = value;
                RaisePropertyChanged(nameof(ProdcuerId));
            }
        }

        public string ProdcuerName
        {
            get => producer.Name;
            set
            {
                producer.Name = value;
                RaisePropertyChanged(nameof(ProdcuerName));
            }
        }

        public string ProdcuerAddress
        {
            get => producer.Address;
            set
            {
                producer.Address = value;
                RaisePropertyChanged(nameof(ProdcuerAddress));
            }
        }
        private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }   
}
