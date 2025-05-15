using System.ComponentModel;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Windows;
using Menu_Restaurant_2.Commands;
using System.Windows.Controls;
using Menu_Restaurant_2.ViewModel.DBFunctions;

namespace Menu_Restaurant_2.ViewModel
{
    public class AddDishVM : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string PortionSize { get; set; }
        public string Category { get; set; }
        public string Info { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public bool IsMenu { get; set; }
        public int Stock { get; set; }



        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }


        public AddDishVM()
        {
            ConfirmCommand = new RelayCommands(AddFood);
            CancelCommand = new RelayCommands(Cancel);
        }

        private void AddFood()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(PortionSize) ||
                string.IsNullOrWhiteSpace(Category) || Price <= 0)
            {
                MessageBox.Show("Please complete all required fields.");
                return;
            }

            try
            {
                Sql.AddFood(Name, Price, Category, Info, PortionSize, Image, IsMenu, Stock);
                MessageBox.Show("Food added successfully.");
            }
            
            catch (SqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
            }
        }
        private void Cancel()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is View.AddDish)
                {
                    window.Close();
                    break;
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
