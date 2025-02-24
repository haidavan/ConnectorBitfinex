using ConnectorBitfinex;
using System.Windows;
using System.Windows.Automation;
using WpfGUI.ViewModels;
using WpfGUI.WindowsForFetchingData;

namespace WpfGUI
{
    public partial class MainWindow : Window
    {
        private readonly ITestConnector _connector;

        public TradesViewModel TradesViewModel { get; set; }
        public CandlesViewModel CandlesViewModel { get; set; }
        public TickersViewModel TickersViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            _connector = new Connector();

            TradesViewModel = new TradesViewModel();
            CandlesViewModel = new CandlesViewModel();
            TickersViewModel = new TickersViewModel();

            DataContext = this;
        }

        private void FetchTradesButton_Click(object sender, RoutedEventArgs e)
        {
            var tradesWindow = new TradesWindow(_connector, TradesViewModel);
            tradesWindow.Show();
        }

        private void FetchCandlesButton_Click(object sender, RoutedEventArgs e)
        {
            var candlesWindow = new CandlesWindow(_connector, CandlesViewModel);
            candlesWindow.Show();
        }

        private void FetchTickersButton_Click(object sender, RoutedEventArgs e)
        {
            var tickersWindow = new TickersWindow(_connector, TickersViewModel);
            tickersWindow.Show();
        }
        private async void ConvertButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            var availableExchangePairs = await _connector.GetAvailableExchangePairs();
            string currency = CurrencyTextBox.Text;

            if (decimal.TryParse(BtcTextBox.Text, out decimal btcAmount) &&
                decimal.TryParse(XrpTextBox.Text, out decimal xrpAmount) &&
                decimal.TryParse(XmrTextBox.Text, out decimal xmrAmount) &&
                decimal.TryParse(DshTextBox.Text, out decimal dashAmount))
            {
                try
                {
                    var btcInResCurrency = await ConvertBTCAsync(availableExchangePairs,btcAmount, currency);
                    var xrpInResCurrency = await ConvertToCurrencyAsync(availableExchangePairs, "XRP", xrpAmount, currency);
                    var xmrInResCurrency = await ConvertToCurrencyAsync(availableExchangePairs, "XMR", xmrAmount, currency);
                    var dshInResCurrency = await ConvertToCurrencyAsync(availableExchangePairs, "DSH", dashAmount, currency);

                    ResultTextBox.Text = (btcInResCurrency + xrpInResCurrency + xmrInResCurrency + dshInResCurrency).ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please enter valid amounts for BTC, XRP, XMR, and DASH.");
            }
        }
        private async Task<decimal> ConvertBTCAsync(IEnumerable<string> availableExchangePairs, decimal amount, string toCurrency)
        {
            if ("BTC" == toCurrency)
            {
                return amount;
            }
            if (availableExchangePairs.Contains($"BTC{toCurrency}"))
            {
                var ticker = await _connector.GetTickerAsync($"BTC{toCurrency}");
                return ticker.LastPrice * amount;
            }
            if (availableExchangePairs.Contains($"{toCurrency}BTC"))
            {
                var ticker = await _connector.GetTickerAsync($"{toCurrency}BTC");
                return amount / ticker.LastPrice;
            }
            throw new Exception($"No valid exchange pair found for BTC to {toCurrency}");
        }
        private async Task<decimal> ConvertToCurrencyAsync(IEnumerable<string> availableExchangePairs, string fromCurrency, decimal amount, string toCurrency)
        {
            if (fromCurrency == toCurrency)
            {
                return amount;
            }

            string exchangePair = $"{fromCurrency}{toCurrency}";
            if (availableExchangePairs.Contains(exchangePair))
            {
                var ticker = await _connector.GetTickerAsync(exchangePair);
                return ticker.LastPrice * amount;
            }
            else
            {
                // Convert fromCurrency to BTC
                string toBtcPair = $"{fromCurrency}BTC";
                if (availableExchangePairs.Contains(toBtcPair))
                {
                    var tickerToBtc = await _connector.GetTickerAsync(toBtcPair);
                    decimal amountInBtc = tickerToBtc.LastPrice * amount;

                    // Convert BTC to toCurrency
                    string btcToPair = $"{toCurrency}BTC";
                    if (availableExchangePairs.Contains(btcToPair))
                    {
                        var tickerBtcTo = await _connector.GetTickerAsync(btcToPair);
                        return amountInBtc / tickerBtcTo.LastPrice;
                    }
                }

                throw new Exception($"No valid exchange pair found for {fromCurrency} to {toCurrency}");
            }
        }
    }
}
