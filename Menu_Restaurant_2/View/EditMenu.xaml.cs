using Menu_Restaurant_2.ViewModel;
using System.Windows;

namespace Menu_Restaurant_2.View
{
    
    public partial class EditMenu : Window
    {
        public EditMenu()
        {
            InitializeComponent();
            this.DataContext = new EditMenuVM();
        }
    }
}
