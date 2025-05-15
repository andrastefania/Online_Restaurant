using Menu_Restaurant_2.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Menu_Restaurant_2.View
{
    /// <summary>
    /// Interaction logic for DeleteMenu.xaml
    /// </summary>
    public partial class DeleteMenu : Window
    {
        public DeleteMenu()
        {
            InitializeComponent();
            DataContext = new DeleteMenuVM();

        }
    }
}
