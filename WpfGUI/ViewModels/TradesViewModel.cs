using System.Collections.ObjectModel;
using System.ComponentModel;
using WpfGUI.Models;

namespace WpfGUI.ViewModels
{
    public class TradesViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<TradeModel> _trades;

        public TradesViewModel()
        {
            Trades = new ObservableCollection<TradeModel>();
        }

        public ObservableCollection<TradeModel> Trades
        {
            get => _trades;
            set
            {
                _trades = value;
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