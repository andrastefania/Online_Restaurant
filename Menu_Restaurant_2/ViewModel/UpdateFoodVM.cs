using Menu_Restaurant_2.Commands;
using Menu_Restaurant_2.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace Menu_Restaurant_2.ViewModel
{
    public class UpdateFoodVM : INotifyPropertyChanged
    {
        public ObservableCollection<Food> Dishes { get; set; }
       
        private Food _selectedDish;
        public Food SelectedDish
        {
            get => _selectedDish;
            set
            {
                _selectedDish = value;
                OnPropertyChanged(nameof(SelectedDish));

                if (_selectedDish != null)
                {
                    Price = _selectedDish.Price;
                    PortionSize = _selectedDish.Portion_Size;
                    Info = _selectedDish.Info;
                }
            }
        }

        private decimal _price;
        public decimal Price
        {
            get => _price;
            set { _price = value; OnPropertyChanged(nameof(Price)); }
        }

        private string _portionSize;
        public string PortionSize
        {
            get => _portionSize;
            set { _portionSize = value; OnPropertyChanged(nameof(PortionSize)); }
        }

        private string _info;
        public string Info
        {
            get => _info;
            set { _info = value; OnPropertyChanged(nameof(Info)); }
        }

        public ICommand UpdateCommand { get; }
        public ICommand CancelCommand { get; }

        public UpdateFoodVM()
        {
            Dishes = new ObservableCollection<Food>();
            LoadDishes();

            UpdateCommand = new RelayCommands(UpdateDish);
            CancelCommand = new RelayCommands(CloseWindow);
        }

        private void LoadDishes()
        {
            Dishes.Clear();
            var all = DBFunctions.Sql.GetAllDishesWithDetails();
            foreach (var dish in all)
                Dishes.Add(dish);

        }

        private void UpdateDish()
        {
            if (SelectedDish == null || Price <= 0 || string.IsNullOrWhiteSpace(PortionSize))
            {
                MessageBox.Show("Please select a dish and fill in all required fields.");
                return;
            }

            try
            {
                DBFunctions.Sql.UpdateFood(SelectedDish.Id, Price, PortionSize, Info);
                MessageBox.Show("Dish updated successfully.");
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error updating dish: " + ex.Message);
            }
        }

        private void CloseWindow()
        {
            foreach (Window win in Application.Current.Windows)
            {
                if (win is View.UpdateDish)
                {
                    win.Close();
                    break;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
