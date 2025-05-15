using Menu_Restaurant_2.Model;
using Menu_Restaurant_2.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Menu_Restaurant_2.View
{
    public partial class MenuPages : Window
    {
        public MenuPages()
        {
            InitializeComponent();

            try
            {
                var viewModel = new MenuPagesVM();
                this.DataContext = viewModel;
                viewModel.NavigateToProfilePage += OpenProfile;
                viewModel.NavigateToOrderPage += OpenOrder;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in MenuPages: " + ex.Message);
            }
            
        }
        private void OpenProfile()
        {
            try
            {
                var userService = new Menu_Restaurant.Json.UserService();
                var user = userService.LoadUser();

                if (user == null || user.Id == 0)
                {
                    MessageBox.Show("Niciun utilizator conectat.");
                    return;
                }

                if (user.Type.Equals("customer", StringComparison.OrdinalIgnoreCase))
                {
                    var window = new UserAccount(user);
                    window.Show();
                }
                else if (user.Type.Equals("employee", StringComparison.OrdinalIgnoreCase))
                {
                    var window = new EmployeeAccount(user);
                    window.Show();
                }
                else
                {
                    MessageBox.Show("Tip de utilizator necunoscut: " + user.Type);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la deschiderea contului: " + ex.Message);
            }
        }
        private void OpenOrder()
        {
            try
            {
                var userService = new Menu_Restaurant.Json.UserService();
                var user = userService.LoadUser();

                if (user == null || user.Id == 0)
                {
                    MessageBox.Show("Niciun utilizator conectat.");
                    return;
                }

                if (user.Type.Equals("customer", StringComparison.OrdinalIgnoreCase))
                {
                    
                    if (this.DataContext is MenuPagesVM vm)
                    {
                        var window = new PlaceOrder(vm.CartItems);
                        window.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Doar clienții pot plasa comenzi.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la deschiderea comenzii: " + ex.Message);
            }
        }

    }
}

