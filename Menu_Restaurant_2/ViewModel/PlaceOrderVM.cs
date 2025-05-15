using Menu_Restaurant_2.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Menu_Restaurant_2.Commands;
using System.Windows;
using System.Linq;
using System.ComponentModel;
using Menu_Restaurant_2.Json;
using Menu_Restaurant.Json;
using System;
using Menu_Restaurant_2.ViewModel.DBFunctions;

namespace Menu_Restaurant_2.ViewModel
{
    public class PlaceOrderVM : INotifyPropertyChanged
    {
        public ObservableCollection<ICartItem> CartItems { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }

        public ICommand PlaceOrderCommand { get; }
        public ICommand RemoveFromCartCommand { get; }
        public ICommand BackCommand { get; }

        private decimal _foodPrice;
        public decimal FoodPrice
        {
            get => _foodPrice;
            set
            {
                _foodPrice = value;
                OnPropertyChanged(nameof(FoodPrice));
            }
        }

        private decimal _transportPrice;
        public decimal TransportPrice
        {
            get => _transportPrice;
            set
            {
                _transportPrice = value;
                OnPropertyChanged(nameof(TransportPrice));
            }
        }

        private decimal _totalPrice;
        public decimal TotalPrice
        {
            get => _totalPrice;
            set
            {
                _totalPrice = value;
                OnPropertyChanged(nameof(TotalPrice));
            }
        }

        public PlaceOrderVM(ObservableCollection<ICartItem> cartItems)
        {
            CartItems = new ObservableCollection<ICartItem>(cartItems);
            CartItems.CollectionChanged += (s, e) => CalculateTotal();
            CalculateTotal();

            PlaceOrderCommand = new RelayCommands(OnPlaceOrder);
            RemoveFromCartCommand = new RelayCommands2<ICartItem>(OnRemoveItem);
            BackCommand = new RelayCommands(OnBack);
        }

        private void OnPlaceOrder()
        {
            var user = new UserService().LoadUser();
            if (user == null)
            {
                MessageBox.Show("No user logged in.");
                return;
            }

            try
            {
                string orderNumber = Sql.GenerateUniqueOrderNumber();
                DateTime estimatedTime = DateTime.Now.AddMinutes(CartItems.Count * 10); // 10 min per item

                Sql.AddOrder(
                    customerId: user.Id,
                    name: Name,
                    phone: Phone,
                    address: Address,
                    foodPrice: FoodPrice,
                    transportPrice: TransportPrice,
                    totalPrice: TotalPrice,
                    orderNumber: orderNumber,
                    estimatedTime: estimatedTime
                );

                foreach (var item in CartItems)
                {
                    if (item is Food food)
                    {
                        Sql.UpdateStock(foodId: food.Id, menuId: 0);
                    }
                    else if (item is Menu menu)
                    {
                        Sql.UpdateStock(foodId: 0, menuId: menu.Id);
                    }
                }

                MessageBox.Show("Comandă plasată cu succes!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la plasarea comenzii: {ex.Message}");
            }
        }

        private void OnRemoveItem(ICartItem item)
        {
            if (item != null && CartItems.Contains(item))
                CartItems.Remove(item);
        }

        private void OnBack()
        {
            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is View.PlaceOrder)?.Close();
        }

        private void CalculateTotal()
        {
            FoodPrice = CartItems.Sum(f => f.Price);

            var config = ConfigurationService.LoadConfig();
            if (FoodPrice < (decimal)config.DeliveryFee.FreeAbove)
                TransportPrice = (decimal)config.DeliveryFee.FeeBelow;
            else
                TransportPrice = 0;

            TotalPrice = FoodPrice + TransportPrice;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
