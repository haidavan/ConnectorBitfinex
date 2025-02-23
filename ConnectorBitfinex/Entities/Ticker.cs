namespace ConnectorBitfinex.Entities;

public class Ticker
{
    /// <summary>
    /// Валютная пара
    /// </summary>
    public string Pair { get; set; }

    /// <summary>
    /// Цена последней наивысшей заявки
    /// </summary>
    public decimal Bid { get; set; }

    /// <summary>
    /// 25 самых высоких размеров заявок
    /// </summary>
    public decimal BidSize { get; set; }
    /// <summary>
    /// Цена последнего наименьшего предложения на продажу
    /// </summary>
    public decimal Ask { get; set; }
    /// <summary>
    /// Сумма 25 самых низких размеров ask
    /// </summary>
    public decimal AskSize { get; set; }

    /// /// <summary>
    /// Сумма изменения последней цены со вчерашнего дня
    /// </summary>
    public decimal DailyChange { get; set; }
    /// <summary>
    /// Относительное изменение последней цены со вчерашнего дня
    /// </summary>
    public decimal DailyChangeRelative { get; set; }

    /// /// <summary>
    /// Цена последней сделки
    /// </summary>
    public decimal LastPrice { get; set; }
    /// <summary>
    /// Дневной объем
    /// </summary>
    public decimal Volume { get; set; }
    /// <summary>
    /// Максимальная цена за день
    /// </summary>
    public decimal High { get; set; }
    /// <summary>
    /// Минимальная цена за день
    /// </summary>
    public decimal Low { get; set; }
}
