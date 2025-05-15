using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using Menu_Restaurant_2.Commands;
using Menu_Restaurant_2.Model;
using Menu_Restaurant_2.ViewModel.DBFunctions;

namespace Menu_Restaurant_2.ViewModel
{
    public class DeleteMenuVM : INotifyPropertyChanged
    {
        public ObservableCollection<Menu> Menus { get; set; }
        public Menu SelectedMenu { get; set; }

        public ICommand DeleteCommand { get; }
        public ICommand CancelCommand { get; }

        public DeleteMenuVM()
        {
            Menus = new ObservableCollection<Menu>();
            LoadMenus();

            DeleteCommand = new RelayCommands(DeleteMenu);
            CancelCommand = new RelayCommands(CloseWindow);
        }

        private void LoadMenus()
        {
            try
            {
                Menus = Sql.GetMenus();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Failed to load menus: " + ex.Message);
            }
        }

        private void DeleteMenu()
        {
            if (SelectedMenu == null)
            {
                MessageBox.Show("Please select a menu to delete.");
                return;
            }

            try
            {
                Sql.DeleteMenuById(SelectedMenu.Id);

                MessageBox.Show($"Menu '{SelectedMenu.Name}' was deleted.");
                Menus.Remove(SelectedMenu);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Failed to delete menu: " + ex.Message);
            }
        }

        private void CloseWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is View.DeleteMenu)
                {
                    window.Close();
                    break;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
