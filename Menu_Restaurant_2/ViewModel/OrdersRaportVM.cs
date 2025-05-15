using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using Menu_Restaurant_2.Commands;
using Menu_Restaurant_2.ViewModel.DBFunctions;

namespace Menu_Restaurant_2.ViewModel
{
    public class OrderItem
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
    }

    public class OrdersRaportVM : INotifyPropertyChanged
    {
        public DateTime StartDate { get; set; } = DateTime.Today.AddDays(-7);
        public DateTime EndDate { get; set; } = DateTime.Today;

        public ObservableCollection<OrderItem> Orders { get; set; } 

        public ICommand GenerateCommand { get; }

        public OrdersRaportVM()
        {
            GenerateCommand = new RelayCommands(GenerateReport);
            Orders = new ObservableCollection<OrderItem>();
        }

        private void GenerateReport()
        {
            Orders.Clear();
            try
            {
                var ordersList = Sql.GenerateOrderReport(StartDate, EndDate);

                
                foreach (var order in ordersList)
                {
                    Orders.Add(order);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
