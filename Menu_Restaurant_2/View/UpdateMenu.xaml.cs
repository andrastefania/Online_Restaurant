using Menu_Restaurant_2.Model;
using Menu_Restaurant_2.ViewModel;
using System.Windows;
using System.Windows.Controls;


namespace Menu_Restaurant_2.View
{
    /// <summary>
    /// Interaction logic for UpdateMenu.xaml
    /// </summary>
    public partial class UpdateMenu : Window
    {
        private ViewModel.UpdateMenuVM VM => DataContext as ViewModel.UpdateMenuVM;
        public UpdateMenu()
        {
            InitializeComponent();
            DataContext = new UpdateMenuVM();
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems)
                VM.SelectedFoods.Add(item as Food);

            foreach (var item in e.RemovedItems)
                VM.SelectedFoods.Remove(item as Food);
        }
    }
}
