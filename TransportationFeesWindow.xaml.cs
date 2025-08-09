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

namespace BudgetTracker
{
    /// <summary>
    /// Interaction logic for TransportationFeesWindow.xaml
    /// </summary>
    public partial class TransportationFeesWindow : Window
    {
        MainWindow main;
        public string[] sources = { "Aircon-MM.png", "Ordinary-MM.png" };
        public int index=0;
        public TransportationFeesWindow(MainWindow main)
        {
            this.main = main;
            InitializeComponent();
        }

        private void PrevClick(object sender, RoutedEventArgs e)
        {
            if (index-1 < 0) { return; }

            index -= 1;
            TransportName.Content = "PUB(Aircon) - Fees";
            ImageFees.Source = new BitmapImage(new Uri($"Fares/{sources[index]}", UriKind.Relative));
        }

        private void NextClick(object sender, RoutedEventArgs e)
        {
            if (index + 1 == sources.Length) { return; }

            index += 1;
            TransportName.Content = "PUB(Ordinary) - Fees";
            ImageFees.Source = new BitmapImage(new Uri($"Fares/{sources[index]}", UriKind.Relative));
        }
    }
}
