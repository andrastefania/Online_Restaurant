using Menu_Restaurant_2.ViewModel;
using System.Windows;

namespace Menu_Restaurant_2.View
{
    public partial class CancelOrderBox : Window
    {
        public string OrderNumber => (DataContext as CancelOrderVM)?.OrderNumber;

        public CancelOrderBox()
        {
            InitializeComponent();

            var vm = new CancelOrderVM();
            vm.RequestClose += result =>
            {
                DialogResult = result;
                Close();
            };

            DataContext = vm;
        }
    }
}
