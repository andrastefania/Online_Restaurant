using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using Menu_Restaurant_2.Commands;
using Menu_Restaurant_2.Model;
using Menu_Restaurant_2.ViewModel.DBFunctions;

namespace Menu_Restaurant_2.ViewModel
{
    public class ChangeStatusVM : INotifyPropertyChanged
    {
        public ObservableCollection<Orders> Orders { get; set; } = new ObservableCollection<Orders>();
        public Orders SelectedOrder { get; set; }

        public ObservableCollection<string> StatusOptions { get; set; } = new ObservableCollection<string> { "registered", "preparing", "on the way", "delivered" };
        public string NewStatus { get; set; }

        public ICommand UpdateCommand { get; }
        public ICommand CancelCommand { get; }

        public ChangeStatusVM()
        {
            LoadOrders();
            UpdateCommand = new RelayCommands(UpdateStatus);
            CancelCommand = new RelayCommands(CloseWindow);
        }

        private void LoadOrders()
        {
            Orders = Sql.GetActiveOrders();
            OnPropertyChanged(nameof(Orders));
        }

        private void UpdateStatus()
        {
            if (SelectedOrder == null || string.IsNullOrWhiteSpace(NewStatus))
            {
                MessageBox.Show("Please select an order and a new status.");
                return;
            }

            Sql.UpdateOrderStatus(SelectedOrder.Id, NewStatus);
            MessageBox.Show("Order status updated.");
            SelectedOrder.Status = NewStatus;
            OnPropertyChanged(nameof(Orders));
        }

        private void CloseWindow()
        {
            foreach (Window win in Application.Current.Windows)
                if (win is View.ChangeStatus)
                {
                    win.Close();
                    break;
                }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
