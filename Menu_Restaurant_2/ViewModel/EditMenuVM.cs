using Menu_Restaurant_2.Commands;
using System;
using System.Windows;
using System.Windows.Input;
using Menu_Restaurant_2.View;
using System.Linq;

namespace Menu_Restaurant_2.ViewModel
{
    public class EditMenuVM
    {
        public ICommand AddDCommand { get; }
        public ICommand DeleteDCommand { get; }
        public ICommand UpdateDCommand { get; }
        public ICommand AddMCommand { get; }
        public ICommand DeleteMCommand { get; }
        public ICommand UpdateMCommand { get; }
        public ICommand BackCommand { get; }

        public EditMenuVM()
        {
            AddDCommand = new RelayCommands(OpenAddD);
            DeleteDCommand = new RelayCommands(OpenDeleteD);
            UpdateDCommand = new RelayCommands(OpenUpdateD);
            AddMCommand = new RelayCommands(OpenAddM);
            DeleteMCommand = new RelayCommands(OpenDeleteM);
            UpdateMCommand = new RelayCommands(OpenUpdateM);
            BackCommand = new RelayCommands(GoBack);
        }

        private void OpenAddD()
        {
            var window = new AddDish();
            window.ShowDialog();
        }

        private void OpenDeleteD()
        {
            var window = new DeleteDish();
            window.ShowDialog();
        }

        private void OpenUpdateD()
        {
            var window = new UpdateDish();
            window.ShowDialog();
        }
        private void OpenAddM()
        {
            var window = new AddMenu();
            window.ShowDialog();
        }

        private void OpenDeleteM()
        {
            var window = new DeleteMenu();
            window.ShowDialog();
        }

        private void OpenUpdateM()
        {
            var window = new UpdateMenu();
            window.ShowDialog();
        }
        private void GoBack()
        {
            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is EditMenu)
                ?.Close();
        }
    }
}
