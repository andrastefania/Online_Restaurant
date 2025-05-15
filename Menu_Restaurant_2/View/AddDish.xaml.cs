using System.Windows;
using Menu_Restaurant_2.ViewModel;

namespace Menu_Restaurant_2.View
{
    public partial class AddDish : Window
    {
        public AddDish()
        {
            InitializeComponent();
            DataContext = new AddDishVM();
        }
    }
}
