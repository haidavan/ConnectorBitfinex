using ConnectorBitfinex.Entities;
using Newtonsoft.Json.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace ConnectorBitfinex.APIClients
{
    public class WebSocketAPIClient
    {
        private ClientWebSocket _clientWebSocket;

        public event Action<Trade> NewBuyTradeReceived;
        public event Action<Trade> NewSellTradeReceived;
        public event Action<Candle> CandleReceived;

        private Dictionary<string, ChannelInfo> _ChanIdToChannels;

        private class ChannelInfo
        {
            public string type;
            public string info;
        }

        public WebSocketAPIClient()
        {
            _clientWebSocket = new ClientWebSocket();
            _ChanIdToChannels = new Dictionary<string, ChannelInfo>();
        }

        public async Task ConnectToServer()
        {

            await _clientWebSocket.ConnectAsync(new Uri("wss://api-pub.bitfinex.com/ws/2"), CancellationToken.None);
            _ = ReceiveMessages();

        }

        private async Task ReceiveMessages()
        {
            byte[] receiveBuffer = new byte[16384];

            while (_clientWebSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await _clientWebSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string receivedMessage = Encoding.UTF8.GetString(receiveBuffer, 0, result.Count);
                    JObject message = new JObject();
                    try
                    {
                        message = JObject.Parse(receivedMessage);
                        var chanType = message["channel"]?.ToString();
                        if (chanType != null)
                            _ChanIdToChannels[message["chanId"].ToString()] =
                                new ChannelInfo
                                {
                                    type = chanType,
                                    info = chanType == "trades" ? message["symbol"].ToString() : message["key"].ToString(),
                                };
                    }
                    catch (Exception ex)//информация о трейдах/свечах состоит из полезных данных без ключей
                    {
                        JArray msg = JArray.Parse(receivedMessage);
                        if (msg[1].ToString() == "hb") continue;
                        if (_ChanIdToChannels[msg[0].ToString()].type == "trades")
                        {
                            if (msg.Count == 2)
                            {
                                foreach (var trade in Utils.GetTradesFromJson(msg[1].ToString(), _ChanIdToChannels[msg[0].ToString()].info))
                                    if (trade.Amount > 0)
                                    {
                                        NewBuyTradeReceived?.Invoke(trade);
                                    }
                                    else
                                    {
                                        NewSellTradeReceived?.Invoke(trade);
                                    }
                            }
                            else if (msg.Count == 3)
                            {
                                var trade = Utils.GetTradeFromJson(msg[2].ToString(), _ChanIdToChannels[msg[0].ToString()].info);
                                if (trade.Amount > 0)
                                {
                                    NewBuyTradeReceived?.Invoke(trade);
                                }
                                else
                                {
                                    NewSellTradeReceived?.Invoke(trade);
                                }
                            }
                        }
                        else if (_ChanIdToChannels[msg[0].ToString()].type == "candles")
                            try
                            {
                                foreach (var candle in Utils.GetCandlesFromJson(msg[1].ToString(), _ChanIdToChannels[msg[0].ToString()].info))
                                {
                                    CandleReceived?.Invoke(candle);
                                }
                            }
                            catch (Exception e) //exception trying to parse candle array, so it most likely single candle
                            {
                                CandleReceived?.Invoke(Utils.GetCandleFromJson(msg[1].ToString(), _ChanIdToChannels[msg[0].ToString()].info));
                            }
                    }
                }
            }
        }

        private async Task SendMessage(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await _clientWebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        //request for subscribe candles example
        //event: "subscribe",
        //channel: "candles",
        //key: "trade:1m:tBTCUSD"
        public async Task SubscribeCandlesAsync(string pair, string timeframe)
        {
            var msg = new
            {
                @event = "subscribe",
                channel = "candles",
                key = $"trade:{timeframe}:{pair}",
            };
            await SendMessage(JsonSerializer.Serialize(msg));
        }

        //request for subscribe trades example, no max count
        //event: "subscribe", 
        //channel: "trades", 
        //symbol: SYMBOL

        public async Task SubscribeTrades(string pair, int maxCount = 100)
        {
            var msg = new
            {
                @event = "subscribe",
                channel = "trades",
                symbol = $"{pair}",
            };
            await SendMessage(JsonSerializer.Serialize(msg));
        }

        public async Task UnsubscribeCandles(string pair)
        {
            var msg = new
            {
                @event = "unsubscribe",
                chanId = $"{_ChanIdToChannels
                .Where(keyWithValue => keyWithValue.Value.info.Split(":").Last() == pair)
                .Select(keyWithValue => keyWithValue.Key)
                .FirstOrDefault()}",
            };
            await SendMessage(JsonSerializer.Serialize(msg));
        }

        public async Task UnsubscribeTrades(string pair)
        {
            var msg = new
            {
                @event = "unsubscribe",
                chanId = $"{_ChanIdToChannels
                .Where(keyWithValue => keyWithValue.Value.info == pair)
                .Select(keyWithValue => keyWithValue.Key)
                .FirstOrDefault()}",
            };
            await SendMessage(JsonSerializer.Serialize(msg));
        }

        public async Task CloseConnection()
        {
            await _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
        }
    }
}