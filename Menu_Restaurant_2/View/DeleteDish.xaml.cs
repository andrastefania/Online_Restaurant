using System.Windows;
using Menu_Restaurant_2.ViewModel;

namespace Menu_Restaurant_2.View
{
   
    public partial class DeleteDish : Window
    {
        public DeleteDish()
        {
            InitializeComponent();
            DataContext = new DeleteDishVM();
        }
    }
}
