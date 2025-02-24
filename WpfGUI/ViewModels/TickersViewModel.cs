using System.Collections.ObjectModel;
using System.ComponentModel;
using WpfGUI.Models;

namespace WpfGUI.ViewModels
{
    public class TickersViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<TickerModel> _tickers;

        public TickersViewModel()
        {
            Tickers = new ObservableCollection<TickerModel>();
        }

        public ObservableCollection<TickerModel> Tickers
        {
            get => _tickers;
            set
            {
                _tickers = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
