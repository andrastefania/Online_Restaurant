using System.Windows;
using Menu_Restaurant_2.ViewModel;

namespace Menu_Restaurant_2.View
{
    
    public partial class ChangeStatus : Window
    {
        public ChangeStatus()
        {
            InitializeComponent();
            DataContext = new ChangeStatusVM();
        }
    }
}
