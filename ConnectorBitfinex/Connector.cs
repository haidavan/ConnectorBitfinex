using ConnectorBitfinex.Entities;
using ConnectorBitfinex.APIClients;

namespace ConnectorBitfinex;

public class Connector : ITestConnector
{
    private RestAPIClient _restApiClient;
    private WebSocketAPIClient _websocketClient;

    private event Action<Trade> _newBuyTrade;
    private event Action<Trade> _newSellTrade;
    private event Action<Candle> _candleSeriesProcessing;
    public Connector()
    {
        _restApiClient = new RestAPIClient();
        _websocketClient = new WebSocketAPIClient();

        _websocketClient.NewBuyTradeReceived += OnNewBuyTradeReceived;
        _websocketClient.NewSellTradeReceived += OnNewSellTradeReceived;
        _websocketClient.CandleReceived += OnCandleReceived;

        _websocketClient.ConnectToServer().Wait();
    }

    public event Action<Trade> NewBuyTrade
    {
        add { _newBuyTrade += value; }
        remove { _newBuyTrade -= value; }
    }
    public event Action<Trade> NewSellTrade
    {
        add { _newSellTrade += value; }
        remove { _newSellTrade -= value; }
    }

    public event Action<Candle> CandleSeriesProcessing
    {
        add { _candleSeriesProcessing += value; }
        remove { _candleSeriesProcessing -= value; }
    }
    private void OnNewBuyTradeReceived(Trade trade)
    {
        _newBuyTrade?.Invoke(trade);
    }

    private void OnNewSellTradeReceived(Trade trade)
    {
        _newSellTrade?.Invoke(trade);
    }

    private void OnCandleReceived(Candle candle)
    {
        _candleSeriesProcessing?.Invoke(candle);
    }

    public async Task<IEnumerable<Candle>> GetCandleSeriesAsync(string pair, int periodInSec, DateTimeOffset? from, DateTimeOffset? to, long? count)
    {
        return Utils.GetCandlesFromJson(await _restApiClient.GetCandleSeriesAsync(pair, Utils.getAllowedTimeFrames(periodInSec), from, to, count),pair);
    }
    public async Task<IEnumerable<Trade>> GetNewTradesAsync(string pair, int maxCount)
    {
        return Utils.GetTradesFromJson(await _restApiClient.GetTradeAsync(pair, maxCount),pair);
    }

    public async Task<Ticker> GetTickerAsync(string pair)
    {
        return Utils.GetTickerFromJson(await _restApiClient.GetTickerAsync(pair),pair);
    }

    public void SubscribeCandles(string pair, int periodInSec, DateTimeOffset? from, DateTimeOffset? to, long? count)
    {
        _websocketClient.SubscribeCandlesAsync(pair, Utils.getAllowedTimeFrames(periodInSec));
    }

    public void SubscribeTrades(string pair, int maxCount)
    {
       _websocketClient.SubscribeTrades(pair,maxCount);
    }

    public void UnsubscribeCandles(string pair)
    {
        _websocketClient.UnsubscribeCandles(pair);
    }

    public void UnsubscribeTrades(string pair)
    {
       _websocketClient.UnsubscribeTrades(pair);
    }
    
    ~Connector()
    { 
        _websocketClient.CloseConnection();
    }
}
