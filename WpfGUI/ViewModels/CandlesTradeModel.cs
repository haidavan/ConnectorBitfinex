using System.Collections.ObjectModel;
using System.ComponentModel;
using WpfGUI.Models;

namespace WpfGUI.ViewModels
{
    public class CandlesViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<CandleModel> _candles;

        public CandlesViewModel()
        {
            Candles = new ObservableCollection<CandleModel>();
        }

        public ObservableCollection<CandleModel> Candles
        {
            get => _candles;
            set
            {
                _candles = value;
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
