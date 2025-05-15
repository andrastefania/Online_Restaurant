using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Menu_Restaurant_2.Commands;
using Menu_Restaurant_2.Model;
using Menu_Restaurant_2.Json;
using Menu_Restaurant_2.ViewModel.DBFunctions;

namespace Menu_Restaurant_2.ViewModel
{
    public class UpdateMenuVM : INotifyPropertyChanged
    {
        public ObservableCollection<Menu> Menus { get; set; }
        public ObservableCollection<Food> AllFood { get; set; }
        public ObservableCollection<Food> SelectedFoods { get; set; }

        private Menu _selectedMenu;
        public Menu SelectedMenu
        {
            get => _selectedMenu;
            set
            {
                _selectedMenu = value;
                OnPropertyChanged(nameof(SelectedMenu));
                if (_selectedMenu != null)
                {
                    NewName = _selectedMenu.Name;
                    LoadMenuFoods(_selectedMenu.Id);
                }
            }
        }

        private string _newName;
        public string NewName
        {
            get => _newName;
            set { _newName = value; OnPropertyChanged(nameof(NewName)); }
        }

        public ICommand UpdateCommand { get; }
        public ICommand CancelCommand { get; }

        public UpdateMenuVM()
        {
            Menus = new ObservableCollection<Menu>();
            AllFood = new ObservableCollection<Food>();
            SelectedFoods = new ObservableCollection<Food>();

            LoadMenus();
            LoadAllFood();

            UpdateCommand = new RelayCommands(UpdateMenu);
            CancelCommand = new RelayCommands(CloseWindow);
        }

        private void LoadMenus()
        {
            Menus.Clear();
            var menus = Sql.GetMenus();
            foreach (var menu in menus)
                Menus.Add(menu);
        }

        private void LoadAllFood()
        {
            AllFood.Clear();
            var foodList = Sql.GetAllFood();
            foreach (var food in foodList)
                AllFood.Add(food);
        }

        private void LoadMenuFoods(int menuId)
        {
            SelectedFoods.Clear();
            var foods = Sql.GetFoodsForMenu(menuId);
            foreach (var food in foods)
                SelectedFoods.Add(food);
        }

        private void UpdateMenu()
        {
            if (SelectedMenu == null || string.IsNullOrWhiteSpace(NewName) || SelectedFoods.Count == 0)
            {
                MessageBox.Show("Please select a menu, enter a new name, and select at least one food.");
                return;
            }

            try
            {
                var config = ConfigurationService.LoadConfig();
                double discountPercent = config.MenuDiscount?.DiscountPercent ?? 0;
                string portionSize = string.Join("/", SelectedFoods.Select(f => f.Portion_Size));
                decimal totalPrice = SelectedFoods.Sum(f => f.Price);
                decimal discountedPrice = totalPrice - (totalPrice * (decimal)(discountPercent / 100));
                int stock = SelectedFoods.Min(f => f.Stock);
                var foodIds = SelectedFoods.Select(f => f.Id).ToList();

                Sql.UpdateMenu(SelectedMenu.Id, NewName, portionSize, discountedPrice, stock, foodIds);
                MessageBox.Show("Menu updated successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while updating: " + ex.Message);
            }
        }

        private void CloseWindow()
        {
            foreach (Window win in Application.Current.Windows)
            {
                if (win is View.UpdateMenu)
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
