using Menu_Restaurant_2.Model;
using Menu_Restaurant_2.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace Menu_Restaurant_2.View
{
    /// <summary>
    /// Interaction logic for AddMenu.xaml
    /// </summary>
    public partial class AddMenu : Window
    {
        public AddMenu()
        {
            InitializeComponent();
            DataContext = new AddMenuVM();
        }
        private void AllFood_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is AddMenuVM vm)
            {
                foreach (var item in e.AddedItems.OfType<Food>())
                {
                    if (!vm.SelectedFood.Contains(item))
                        vm.SelectedFood.Add(item);
                }

                foreach (var item in e.RemovedItems.OfType<Food>())
                {
                    if (vm.SelectedFood.Contains(item))
                        vm.SelectedFood.Remove(item);
                }
            }
        }

    }
}
