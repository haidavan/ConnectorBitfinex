using ConnectorBitfinex;

namespace ConsoleApp1;

internal class Program
{
    static async Task Main(string[] args)
    {
        //RestAPIClient client = new RestAPIClient();
        ////var t = await client.GetCandleSeriesAsync("tBTCUSD", 60,DateTimeOffset.MinValue,DateTimeOffset.Now,10);
        //var t = await client.GetTickerAsync("tBTCUSD");
        //WebSocketAPIClient client1 = new WebSocketAPIClient();
        //await client1.ConnectToServer();
        //client1.SubscribeTrades("tBTCUSD",100);
        //client1.SubscribeCandlesAsync("tBTCUSD", "1m");
        //int i = 0;
        var con = new Connector();
        con.SubscribeTrades("tBTCUSD", 100);
        con.NewSellTrade += (trade) => { Console.WriteLine(trade.Id); };
        con.NewBuyTrade += (trade) => { Console.WriteLine(trade.Id); };
        while (true) { }
        //foreach (var item in t)
        //{
        //    //Console.WriteLine($"{item.Id} {item.Time} {item.Pair} {item.Price} {item.Amount} {item.Side}");
        //    //Console.WriteLine($"{item.ClosePrice} {item.TotalVolume} {item.TotalPrice}" +
        //       // $" {item.OpenPrice} {item.LowPrice} {item.HighPrice} {item.Pair} {item.OpenTime}");
        //}
    }
}
