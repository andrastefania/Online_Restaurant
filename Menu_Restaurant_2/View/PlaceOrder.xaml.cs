using Menu_Restaurant_2.Model;
using Menu_Restaurant_2.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Windows;


namespace Menu_Restaurant_2.View
{
    /// <summary>
    /// Interaction logic for PlaceOrder.xaml
    /// </summary>
    public partial class PlaceOrder : Window
    {
        public PlaceOrder(ObservableCollection<ICartItem> cartItems)
        {
            InitializeComponent();
            DataContext = new PlaceOrderVM(cartItems);
        }
    }
}
