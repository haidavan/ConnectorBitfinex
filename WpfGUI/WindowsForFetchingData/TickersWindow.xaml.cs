using ConnectorBitfinex;
using ConnectorBitfinex.Entities;
using System;
using System.Threading.Tasks;
using System.Windows;
using WpfGUI.Models;
using WpfGUI.ViewModels;

namespace WpfGUI.WindowsForFetchingData
{
    public partial class TickersWindow : Window
    {
        private readonly ITestConnector _connector;
        public TickersViewModel TickersViewModel { get; set; }

        public TickersWindow(ITestConnector connector, TickersViewModel tickersViewModel)
        {
            InitializeComponent();
            _connector = connector;
            TickersViewModel = tickersViewModel;
        }

        private async void GetTickerButton_Click(object sender, RoutedEventArgs e)
        {
            Ticker ticker;
            try
            {
                ticker = await _connector.GetTickerAsync(PairTextBox.Text);
            }
            catch(Exception ex)
            { 
                MessageBox.Show(ex.Message);
                return;
            }
            TickersViewModel.Tickers.Add(new TickerModel(ticker));
        }
    }
}
