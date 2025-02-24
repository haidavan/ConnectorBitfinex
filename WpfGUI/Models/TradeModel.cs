using ConnectorBitfinex.Entities;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfGUI.Models;
public class TradeModel : INotifyPropertyChanged
{
    private Trade _trade;

    public TradeModel(Trade trade)
    {
        _trade = trade;
    }

    public string Pair
    {
        get => _trade.Pair;
        set
        {
            if (_trade.Pair != value)
            {
                _trade.Pair = value;
                OnPropertyChanged();
            }
        }
    }

    public decimal Price
    {
        get => _trade.Price;
        set
        {
            if (_trade.Price != value)
            {
                _trade.Price = value;
                OnPropertyChanged();
            }
        }
    }

    public decimal Amount
    {
        get => _trade.Amount;
        set
        {
            if (_trade.Amount != value)
            {
                _trade.Amount = value;
                OnPropertyChanged();
            }
        }
    }

    public string Side
    {
        get => _trade.Side;
        set
        {
            if (_trade.Side != value)
            {
                _trade.Side = value;
                OnPropertyChanged();
            }
        }
    }

    public DateTimeOffset Time
    {
        get => _trade.Time;
        set
        {
            if (_trade.Time != value)
            {
                _trade.Time = value;
                OnPropertyChanged();
            }
        }
    }

    public string Id
    {
        get => _trade.Id;
        set
        {
            if (_trade.Id != value)
            {
                _trade.Id = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
