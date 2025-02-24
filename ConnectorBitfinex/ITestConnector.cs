using ConnectorBitfinex.Entities;
using Newtonsoft.Json.Linq;

namespace ConnectorBitfinex;

public interface ITestConnector
{
    #region Rest

    public Task<IEnumerable<Trade>> GetNewTradesAsync(string pair, int maxCount);
    public Task<IEnumerable<Candle>> GetCandleSeriesAsync(string pair, int periodInSec, DateTimeOffset? from, DateTimeOffset? to = null, long? count = 0);

    public Task<Ticker> GetTickerAsync(string pair);

    public Task<IEnumerable<string>> GetAvailableExchangePairs();//список доступных конвертаций
    #endregion

    #region Socket


   public event Action<Trade> NewBuyTrade;
    public event Action<Trade> NewSellTrade;
    public void SubscribeTrades(string pair, int maxCount = 100);
    public void UnsubscribeTrades(string pair);

    public event Action<Candle> CandleSeriesProcessing;
    public void SubscribeCandles(string pair, int periodInSec, DateTimeOffset? from = null, DateTimeOffset? to = null, long? count = 0);
    public void UnsubscribeCandles(string pair);

    #endregion

}
