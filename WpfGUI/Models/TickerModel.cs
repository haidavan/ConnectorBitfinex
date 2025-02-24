using ConnectorBitfinex.Entities;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfGUI.Models
{
    public class TickerModel : INotifyPropertyChanged
    {
        private Ticker _ticker;

        public TickerModel(Ticker ticker)
        {
            _ticker = ticker;
        }

        public string Pair
        {
            get => _ticker.Pair;
            set
            {
                if (_ticker.Pair != value)
                {
                    _ticker.Pair = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal Bid
        {
            get => _ticker.Bid;
            set
            {
                if (_ticker.Bid != value)
                {
                    _ticker.Bid = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal BidSize
        {
            get => _ticker.BidSize;
            set
            {
                if (_ticker.BidSize != value)
                {
                    _ticker.BidSize = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal Ask
        {
            get => _ticker.Ask;
            set
            {
                if (_ticker.Ask != value)
                {
                    _ticker.Ask = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal AskSize
        {
            get => _ticker.AskSize;
            set
            {
                if (_ticker.AskSize != value)
                {
                    _ticker.AskSize = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal DailyChange
        {
            get => _ticker.DailyChange;
            set
            {
                if (_ticker.DailyChange != value)
                {
                    _ticker.DailyChange = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal DailyChangeRelative
        {
            get => _ticker.DailyChangeRelative;
            set
            {
                if (_ticker.DailyChangeRelative != value)
                {
                    _ticker.DailyChangeRelative = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal LastPrice
        {
            get => _ticker.LastPrice;
            set
            {
                if (_ticker.LastPrice != value)
                {
                    _ticker.LastPrice = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal Volume
        {
            get => _ticker.Volume;
            set
            {
                if (_ticker.Volume != value)
                {
                    _ticker.Volume = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal High
        {
            get => _ticker.High;
            set
            {
                if (_ticker.High != value)
                {
                    _ticker.High = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal Low
        {
            get => _ticker.Low;
            set
            {
                if (_ticker.Low != value)
                {
                    _ticker.Low = value;
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
