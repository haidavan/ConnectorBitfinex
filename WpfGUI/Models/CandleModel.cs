using ConnectorBitfinex.Entities;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfGUI.Models
{
    public class CandleModel : INotifyPropertyChanged
    {
        private Candle _candle;

        public CandleModel(Candle candle)
        {
            _candle = candle;
        }

        public string Pair
        {
            get => _candle.Pair;
            set
            {
                if (_candle.Pair != value)
                {
                    _candle.Pair = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal OpenPrice
        {
            get => _candle.OpenPrice;
            set
            {
                if (_candle.OpenPrice != value)
                {
                    _candle.OpenPrice = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal HighPrice
        {
            get => _candle.HighPrice;
            set
            {
                if (_candle.HighPrice != value)
                {
                    _candle.HighPrice = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal LowPrice
        {
            get => _candle.LowPrice;
            set
            {
                if (_candle.LowPrice != value)
                {
                    _candle.LowPrice = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal ClosePrice
        {
            get => _candle.ClosePrice;
            set
            {
                if (_candle.ClosePrice != value)
                {
                    _candle.ClosePrice = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal TotalPrice
        {
            get => _candle.TotalPrice;
            set
            {
                if (_candle.TotalPrice != value)
                {
                    _candle.TotalPrice = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal TotalVolume
        {
            get => _candle.TotalVolume;
            set
            {
                if (_candle.TotalVolume != value)
                {
                    _candle.TotalVolume = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTimeOffset OpenTime
        {
            get => _candle.OpenTime;
            set
            {
                if (_candle.OpenTime != value)
                {
                    _candle.OpenTime = value;
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
}
