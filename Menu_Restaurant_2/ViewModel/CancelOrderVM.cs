using System;
using System.ComponentModel;
using System.Windows.Input;
using Menu_Restaurant_2.Commands;

namespace Menu_Restaurant_2.ViewModel
{
    public class CancelOrderVM: INotifyPropertyChanged
    {
        private string _orderNumber;
        public string OrderNumber
        {
            get => _orderNumber;
            set
            {
                _orderNumber = value;
                OnPropertyChanged(nameof(OrderNumber));
            }
        }

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }

        public event Action<bool?> RequestClose;

        public CancelOrderVM()
        {
            ConfirmCommand = new RelayCommands(OnConfirm);
            CancelCommand = new RelayCommands(OnCancel);
        }

        private void OnConfirm()
        {
            if (!string.IsNullOrWhiteSpace(OrderNumber))
                RequestClose?.Invoke(true);
        }

        private void OnCancel()
        {
            RequestClose?.Invoke(false);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
