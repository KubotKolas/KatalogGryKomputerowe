using KolasinskiMarcinek.KatalogGryKomputerowe.CORE;
using KolasinskiMarcinek.KatalogGryKomputerowe.INTERFACES;
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

namespace KolasinskiMarcinek.KatalogGryKomputerowe.LocalApp
{
    /// <summary>
    /// Logika interakcji dla klasy NewProducer.xaml
    /// </summary>
    public partial class NewProducer : Window
    {
        public NewProducer()
        {
            InitializeComponent();
        }

        public NewProducer(IProducer producer)
        {
            InitializeComponent();

            producerName.Text = producer.Name;
            producerAdress.Text = producer.Address;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            producerName.SelectAll();
            producerName.Focus();
        }

        public string ProducerName => producerName.Text;

        public string ProducerAddress => producerAdress.Text;

        private void Confirm(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(producerName.Text))
            {
                MessageBox.Show("Podaj nazwę twórcy gry.");
                return;
            }

            if (string.IsNullOrWhiteSpace(producerAdress.Text))
            {
                MessageBox.Show("Podaj adres twórcy gry.");
                return;
            }

            DialogResult = true;
            Close();
        }

    }
}
