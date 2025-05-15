using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using Menu_Restaurant_2.Commands;
using Menu_Restaurant_2.ViewModel.DBFunctions;

namespace Menu_Restaurant_2.ViewModel
{
    public class StockItem
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public string StockStatus { get; set; }
    }

    public class StockReportVM : INotifyPropertyChanged
    {
        private int _threshold = 5;
        public int Threshold
        {
            get => _threshold;
            set { _threshold = value; OnPropertyChanged(nameof(Threshold)); }
        }

        public ObservableCollection<StockItem> StockItems { get; set; }
        public ICommand GenerateCommand { get; }

        public StockReportVM()
        {
            GenerateCommand = new RelayCommands(GenerateReport);
            StockItems = new ObservableCollection<StockItem>();
        }

        private void GenerateReport()
        {
            StockItems.Clear();
            try
            {
                var items = Sql.GetStockReport(Threshold);
                foreach (var item in items)
                    StockItems.Add(item);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
