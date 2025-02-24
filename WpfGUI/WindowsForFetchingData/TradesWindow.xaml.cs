using ConnectorBitfinex;
using ConnectorBitfinex.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfGUI.Models;
using WpfGUI.ViewModels;

namespace WpfGUI.WindowsForFetchingData
{
    public partial class TradesWindow : Window
    {
        private readonly ITestConnector _connector;
        public TradesViewModel TradesViewModel { get; set; }

        public TradesWindow(ITestConnector connector,TradesViewModel tradesViewModel)
        {
            InitializeComponent();
            _connector = connector;
            TradesViewModel = tradesViewModel;
        }

        private async void GetTradesButton_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<Trade> trades;
            if (int.TryParse(MaxCountTextBox.Text, out int maxCount))
            {
                try
                {
                    trades = await _connector.GetNewTradesAsync(PairTextBox.Text, maxCount);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                foreach (var trade in trades)
                {
                    TradesViewModel.Trades.Add(new TradeModel(trade));
                }
            }
            else
            {
                MessageBox.Show("invalid Max Count.");
            }
        }

        private void SubscribeTradesButton_Click(object sender, RoutedEventArgs e)
        {           
                _connector.SubscribeTrades(PairTextBox.Text);
                _connector.NewBuyTrade += trade  => TradesViewModel.Trades.Add(new TradeModel(trade));
                _connector.NewSellTrade += trade =>  TradesViewModel.Trades.Add(new TradeModel(trade));
        }

        private void UnsubscribeTradesButton_Click(object sender, RoutedEventArgs e)
        {
            _connector.UnsubscribeTrades(PairTextBox.Text);
        }
    }
}
