using Menu_Restaurant.Json;
using Menu_Restaurant_2.Commands;
using Menu_Restaurant_2.DataBase;
using Menu_Restaurant_2.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Menu_Restaurant_2.View;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Generic;
using System.Data.SqlClient;
using Menu_Restaurant_2.ViewModel.DBFunctions;

namespace Menu_Restaurant_2.ViewModel
{
    public class EmployeeAccountVM: INotifyPropertyChanged
    {
        public ICommand EditCommand { get; }
        public ICommand StockReportCommand { get; }
        public ICommand OrdersReportCommand { get; }
        public ICommand OrderStatusCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand BackCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private RestaurantDbContext _context;
        public event Action NavigateToStartPage;

        private ObservableCollection<Orders> _orderInProgress;
        public ObservableCollection<Orders> OrderInProgress
        {
            get => _orderInProgress;
            set
            {
                _orderInProgress = value;
                OnPropertyChanged(nameof(OrderInProgress));
            }
        }
        public EmployeeAccountVM(Person user)
        {
            LogoutCommand = new RelayCommands(OnLogout);
            EditCommand = new RelayCommands(OnEdit);
            StockReportCommand = new RelayCommands(ShowStockReport);
            OrdersReportCommand = new RelayCommands(ShowOrdersReport);
            OrderStatusCommand = new RelayCommands(OnStatus);
            BackCommand = new RelayCommands(OnBack);
            _context = new RestaurantDbContext();
            OrderInProgress = new ObservableCollection<Orders>();
            LoadOrders();

        }
        private void OnLogout()
        {
            var service = new Menu_Restaurant.Json.UserService();
            service.ClearUser();

            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is EmployeeAccount)?.Close();
            var startWindow = new StartWindow();
            startWindow.Show();
            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is MenuPages)?.Close();
        }

        private void OnEdit()
        {
            var window = new EditMenu();
            window.ShowDialog();
        }

        private void OnStatus()
        {
            var window = new ChangeStatus();
            window.DataContext = new ChangeStatusVM();
            window.ShowDialog();
        }
        private void OnBack()
        {
            Application.Current.Windows
            .OfType<Window>()
            .FirstOrDefault(w => w is EmployeeAccount)?.Close();
        }

        

        private void LoadOrders()
        {
            try
            {
                var tempList = Sql.LoadOrders();

                // Actualizăm ObservableCollection cu comenzile obținute
                OrderInProgress = new ObservableCollection<Orders>(
                    tempList.OrderByDescending(o => o.Date)
                );
            
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load orders: " + ex.Message);
            }
        }
        private void ShowStockReport()
        {

            var window = new StockRaport();
            window.DataContext = new StockReportVM();
            window.Show();


        }
        private void ShowOrdersReport()
        {
            var window = new OrdersRaport();
            window.DataContext = new OrdersRaportVM();
            window.Show();
        }

    }
}
