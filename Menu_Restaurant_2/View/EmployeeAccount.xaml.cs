using Menu_Restaurant_2.Model;
using System.Windows;


namespace Menu_Restaurant_2.View
{
    /// <summary>
    /// Interaction logic for EmployAccount.xaml
    /// </summary>
    public partial class EmployeeAccount : Window
    {
        public EmployeeAccount(Person user)
        {
            InitializeComponent();
            DataContext = new Menu_Restaurant_2.ViewModel.EmployeeAccountVM(user);
        }
        
    }
}
