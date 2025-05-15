using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using Menu_Restaurant_2.Commands;
using Menu_Restaurant_2.DataBase;
using Menu_Restaurant_2.Model;
using System.Linq;
using Menu_Restaurant_2.ViewModel.DBFunctions;


namespace Menu_Restaurant_2.ViewModel
{
    public class AddMenuVM : INotifyPropertyChanged
    {
        public ObservableCollection<Food> AllFood { get; set; }
        public ObservableCollection<Food> SelectedFood { get; set; }

        private string _name;
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        private string _category;
        public string Category
        {
            get => _category;
            set { _category = value; OnPropertyChanged(nameof(Category)); }
        }

        private string _image;
        public string Image
        {
            get => _image;
            set { _image = value; OnPropertyChanged(nameof(Image)); }
        }

        private string _info;
        public string Info
        {
            get => _info;
            set { _info = value; OnPropertyChanged(nameof(Info)); }
        }

        public ICommand AddCommand { get; }
        public ICommand CancelCommand { get; }

        public AddMenuVM()
        {
            AllFood = new ObservableCollection<Food>();
            SelectedFood = new ObservableCollection<Food>();

            LoadAllFood();
            
            AddCommand = new RelayCommands(AddMenu);
            CancelCommand = new RelayCommands(Cancel);
        }

        private void LoadAllFood()
        {
            try
            {
                using (var context = new RestaurantDbContext())
                {
                    var foodList = context.Food.ToList();
                    foreach (var food in foodList)
                    {
                        AllFood.Add(food);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB error: " + ex.Message);
            }
        }

        private void AddMenu()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Category) || SelectedFood.Count == 0)
            {
                MessageBox.Show("Please fill in required fields and select at least one food item.");
                return;
            }

            try
            {
                var config = Menu_Restaurant_2.Json.ConfigurationService.LoadConfig();
                double discountPercent = config.MenuDiscount?.DiscountPercent ?? 0;
                string portionSize = string.Join("/", SelectedFood.Select(f => f.Portion_Size));
                decimal totalPrice = SelectedFood.Sum(f => f.Price);
                decimal discountedPrice = totalPrice - (totalPrice * (decimal)(discountPercent / 100));
                int stock = SelectedFood.Min(f => f.Stock);

                Sql.AddMenu(Name, Category, portionSize, discountedPrice, stock, Image, Info, SelectedFood.ToList());

                MessageBox.Show("Menu added successfully.");
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
                if (window is View.AddMenu)
                {
                    window.Close();
                    break;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
