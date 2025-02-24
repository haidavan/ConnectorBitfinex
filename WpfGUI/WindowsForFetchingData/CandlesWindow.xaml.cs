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
    public partial class CandlesWindow : Window
    {
        private readonly ITestConnector _connector;
        public CandlesViewModel CandlesViewModel { get; set; }

        public CandlesWindow(ITestConnector connector, CandlesViewModel candlesViewModel)
        {
            InitializeComponent();
            _connector = connector;
            CandlesViewModel = candlesViewModel;
        }

        private async void GetCandlesButton_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<Candle> candles;
            if (int.TryParse(PeriodTextBox.Text, out int periodInSec) &&
                DateTimeOffset.TryParse(FromTextBox.Text, out DateTimeOffset from) &&
                DateTimeOffset.TryParse(ToTextBox.Text, out DateTimeOffset to) &&
                long.TryParse(CountTextBox.Text, out long count))
            {
                try
                {
                    candles = await _connector.GetCandleSeriesAsync(PairTextBox.Text, periodInSec, from, to, count);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                foreach (var candle in candles)
                {
                    CandlesViewModel.Candles.Add(new CandleModel(candle));
                }
            }
            else
            {
                MessageBox.Show("invalid values for Period, From, To, or Count.");
            }
        }

        private void SubscribeCandlesButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(PeriodTextBox.Text, out int periodInSec))
            {
                _connector.SubscribeCandles(PairTextBox.Text, periodInSec);
                _connector.CandleSeriesProcessing += candle =>
                    Dispatcher.Invoke(() => CandlesViewModel.Candles.Add(new CandleModel(candle)));
            }
            else
            {
                MessageBox.Show("invalid Period.");
            }
        }

        private void UnsubscribeCandlesButton_Click(object sender, RoutedEventArgs e)
        {
            _connector.UnsubscribeCandles(PairTextBox.Text);
        }
    }
}
