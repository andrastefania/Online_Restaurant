using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Menu_Restaurant_2.Commands;
using Menu_Restaurant_2.DataBase;
using Menu_Restaurant_2.Model;

namespace Menu_Restaurant_2.ViewModel
{
    public class MenuPagesVM : INotifyPropertyChanged
    {

        public ObservableCollection<string> FilterOptions { get; set; }
        public ObservableCollection<string> OrderOptions { get; set; }
        public ObservableCollection<ICartItem> DisplayItems { get; set; }
        private List<Food> AllFood { get; set; }
        private List<Menu> AllMenus { get; set; }

        public ObservableCollection<ICartItem> CartItems { get; set; }

        public ICommand OpenProfileCommand { get; }
        public ICommand OpenOrderCommand { get; }
        public ICommand AddToCartCommand { get; }

        public event Action NavigateToProfilePage;
        public event Action NavigateToOrderPage;

        private bool _isCustomer;
        public bool IsCustomer
        {
            get => _isCustomer;
            set { _isCustomer = value; OnPropertyChanged(nameof(IsCustomer)); }
        }

        private string _selectedFilter;
        public string SelectedFilter
        {
            get => _selectedFilter;
            set { _selectedFilter = value; OnPropertyChanged(nameof(SelectedFilter)); UpdateDisplayedItems(); }
        }

        private string _selectedOrder;
        public string SelectedOrder
        {
            get => _selectedOrder;
            set { _selectedOrder = value; OnPropertyChanged(nameof(SelectedOrder)); UpdateDisplayedItems(); }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(nameof(SearchText)); UpdateDisplayedItems(); }
        }

        public MenuPagesVM()
        {
            FilterOptions = new ObservableCollection<string>
            {
                "All", "Breakfast", "Main Course", "Dessert", "Vegetarian", "Drink", "Starter", "Soup"
            };

            OrderOptions = new ObservableCollection<string>
            {
                "Name (A-Z)", "Name (Z-A)", "Price (Low to High)", "Price (High to Low)"
            };

            SelectedFilter = FilterOptions[0];
            SelectedOrder = OrderOptions[0];

            DisplayItems = new ObservableCollection<ICartItem>();
            CartItems = new ObservableCollection<ICartItem>();
            AddToCartCommand = new RelayCommands2<ICartItem>(AddToCart);

            try
            {
                using (var context = new RestaurantDbContext())
                {
                    AllFood = context.Food.ToList();
                    AllMenus = context.Menu.Where(m => m.Stock > 0).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB error: " + (ex.InnerException?.Message ?? ex.Message));
            }

            UpdateDisplayedItems();

            OpenProfileCommand = new RelayCommands(OpenProfile);
            OpenOrderCommand = new RelayCommands(OpenOrder);

            var userService = new Menu_Restaurant.Json.UserService();
            var user = userService.LoadUser();
            IsCustomer = user != null && user.Type.Equals("customer", StringComparison.OrdinalIgnoreCase);
        }

        private void UpdateDisplayedItems()
        {
            if (AllFood == null || AllMenus == null) return;

            var filteredFood = AllFood.Where(f => f.Stock > 0);
            var filteredMenus = AllMenus.Where(m => m.Stock > 0);

            if (SelectedFilter != "All")
            {
                filteredFood = filteredFood.Where(f => f.Category.Equals(SelectedFilter, StringComparison.OrdinalIgnoreCase));
                filteredMenus = filteredMenus.Where(m => m.Category.Equals(SelectedFilter, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filteredFood = filteredFood.Where(f => f.Name.ToLower().Contains(SearchText.ToLower()));
                filteredMenus = filteredMenus.Where(m => m.Name.ToLower().Contains(SearchText.ToLower()));
            }

            switch (SelectedOrder)
            {
                case "Name (A-Z)":
                    filteredFood = filteredFood.OrderBy(f => f.Name);
                    filteredMenus = filteredMenus.OrderBy(m => m.Name);
                    break;
                case "Name (Z-A)":
                    filteredFood = filteredFood.OrderByDescending(f => f.Name);
                    filteredMenus = filteredMenus.OrderByDescending(m => m.Name);
                    break;
                case "Price (Low to High)":
                    filteredFood = filteredFood.OrderBy(f => f.Price);
                    filteredMenus = filteredMenus.OrderBy(m => m.Price);
                    break;
                case "Price (High to Low)":
                    filteredFood = filteredFood.OrderByDescending(f => f.Price);
                    filteredMenus = filteredMenus.OrderByDescending(m => m.Price);
                    break;
            }

            DisplayItems.Clear();
            foreach (var item in filteredFood) DisplayItems.Add(item);
            foreach (var item in filteredMenus) DisplayItems.Add(item);
        }

        private void AddToCart(ICartItem item)
        {
            if (item != null)
            {
                CartItems.Add(item);
            }
        }

        private void OpenProfile() => NavigateToProfilePage?.Invoke();
        private void OpenOrder() => NavigateToOrderPage?.Invoke();

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
