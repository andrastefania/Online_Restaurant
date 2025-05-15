
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Menu_Restaurant_2.Model;
using System.Windows;
using Menu_Restaurant_2.Commands;
using System.Windows.Controls;
using Menu_Restaurant_2.View;
using System.ComponentModel;
using System.Data.SqlClient;
using Microsoft.VisualBasic;
using System.Windows.Shell;
using Menu_Restaurant_2.ViewModel.DBFunctions;



namespace Menu_Restaurant.ViewModel
{
    public class UserAccountVM : INotifyPropertyChanged

    {
        public ICommand CancelOrderCommand { get; }
        public ICommand TrackOrderCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand BackCommand { get; }
        public string UserName { get; set; }

        private ObservableCollection<Orders> _orderHistory;
        public ObservableCollection<Orders> OrderHistory
        {
            get => _orderHistory;
            set
            {
                _orderHistory = value;
                OnPropertyChanged(nameof(OrderHistory));
            }
        }

        public UserAccountVM(Person user)
        {
            CancelOrderCommand = new RelayCommands(OnCancelOrder);
            LogoutCommand = new RelayCommands(OnLogout);
            BackCommand = new RelayCommands(OnBack);
            UserName = user.Name;
            OnPropertyChanged(nameof(UserName));

            OrderHistory = new ObservableCollection<Orders>();
            LoadOrders(user.Id);
        }
        private void OnLogout()
        {
            var service = new Menu_Restaurant.Json.UserService();
            service.ClearUser();
            
            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is UserAccount)?.Close();
            var startWindow = new StartWindow(); 
            startWindow.Show();
            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is MenuPages)?.Close();
        }

        private void OnCancelOrder()
        {
            var dialog = new CancelOrderBox();
            if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.OrderNumber))
            {
                string input = dialog.OrderNumber;

                try
                {
                    var user = new Menu_Restaurant.Json.UserService().LoadUser();
                    bool success = Sql.CancelOrder(input, user.Id);

                    if (!success)
                        MessageBox.Show("The order was not found or does not belong to your account.");
                    else
                        MessageBox.Show("Order successfully canceled.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while canceling the order: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Cancelation aborted or no order number entered.");
            }
        }


        private void OnBack()
        {
            Application.Current.Windows
                 .OfType<Window>()
                 .FirstOrDefault(w => w is UserAccount)?.Close();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void LoadOrders(int customerId)
        {
            try
            {
                var orders = Sql.GetOrdersForCustomer(customerId);
                foreach (var order in orders.OrderByDescending(o => o.Date))
                    OrderHistory.Add(order);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load orders: " + ex.Message);
            }
        }

    }
}

