using System.Globalization;
using System.Text.Json;
using ConnectorBitfinex.Entities;

namespace ConnectorBitfinex;

public static class Utils
{
    public static string getAllowedTimeFrames(int periodInSec)
    {
        switch (periodInSec)
        {
            case 60: return "1m";
            case 120: return "2m";
            default:
                throw new Exception();
        }
    }
    public static IEnumerable<Candle> GetCandlesFromJson(string json, string pair)
    {
        return JsonSerializer.Deserialize<List<List<object>>>(json)
           .Select(candle =>
           {
               var closePrice = decimal.Parse(candle[2].ToString(), NumberStyles.Float,
                       CultureInfo.InvariantCulture);
               var totalVolume = decimal.Parse(candle[5].ToString(), NumberStyles.Float,
                       CultureInfo.InvariantCulture);
               return new Candle
               {
                   OpenTime = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(candle[0].ToString())),
                   OpenPrice = decimal.Parse(candle[1].ToString(), NumberStyles.Float,
                       CultureInfo.InvariantCulture),
                   ClosePrice = closePrice,
                   HighPrice = decimal.Parse(candle[3].ToString(), NumberStyles.Float,
                       CultureInfo.InvariantCulture),
                   LowPrice = decimal.Parse(candle[4].ToString(), NumberStyles.Float,
                       CultureInfo.InvariantCulture),
                   TotalVolume = totalVolume,
                   Pair = pair,
                   TotalPrice = totalVolume * closePrice,
               };
           });
    }
    public static IEnumerable<Trade> GetTradesFromJson(string json, string pair)
    {
        return JsonSerializer.Deserialize<List<List<object>>>(json)
            .Select(trade =>
            {
                var amount = decimal.Parse(trade[2].ToString(), NumberStyles.Float,
                    CultureInfo.InvariantCulture);
                return new Trade
                {
                    Id = trade[0].ToString(),
                    Time = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(trade[1].ToString())),
                    Amount = amount,
                    Price = decimal.Parse(trade[3].ToString(), NumberStyles.Float,
                    CultureInfo.InvariantCulture),
                    Pair = pair,
                    Side = amount > 0 ? "buy" : "sell",
                };
            }
        );
    }
    public static Ticker GetTickerFromJson(string json, string pair)
    {
        var tickerdata = JsonSerializer.Deserialize<List<decimal>>(json);
        return new Ticker
        {
            Bid = tickerdata[0],
            BidSize = tickerdata[1],
            Ask = tickerdata[2],
            AskSize = tickerdata[3],
            DailyChange = tickerdata[4],
            DailyChangeRelative = tickerdata[5],
            LastPrice = tickerdata[6],
            Volume = tickerdata[7],
            High = tickerdata[8],
            Low = tickerdata[9],
            Pair = pair,
        };
    }
    public static Trade GetTradeFromJson(string json, string pair)
    {
        var tradedata = JsonSerializer.Deserialize<List<object>>(json);
        var amount = decimal.Parse(tradedata[2].ToString(), NumberStyles.Float,
                    CultureInfo.InvariantCulture);
        return new Trade
        {
            Id = tradedata[0].ToString(),
            Time = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(tradedata[1].ToString())),
            Amount = amount,
            Price = decimal.Parse(tradedata[3].ToString(), NumberStyles.Float,
            CultureInfo.InvariantCulture),
            Pair = pair,
            Side = amount > 0 ? "buy" : "sell",
        };
    }
}
