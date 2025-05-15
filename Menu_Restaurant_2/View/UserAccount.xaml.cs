using Menu_Restaurant_2.Model;
using System.Windows;


namespace Menu_Restaurant_2.View
{
    /// <summary>
    /// Interaction logic for UserAccount.xaml
    /// </summary>
    public partial class UserAccount : Window
    {
        public UserAccount(Person user)
        {
            InitializeComponent();
            DataContext = new Menu_Restaurant.ViewModel.UserAccountVM(user);
        }
    }
}
