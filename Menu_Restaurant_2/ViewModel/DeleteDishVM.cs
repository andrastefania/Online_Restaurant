using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using Menu_Restaurant_2.Commands;
using Menu_Restaurant_2.Model;
using System;
using Menu_Restaurant_2.ViewModel.DBFunctions;

namespace Menu_Restaurant_2.ViewModel
{
    public class DeleteDishVM : INotifyPropertyChanged
    {
        public ObservableCollection<Food> Dishes { get; set; }
        public Food SelectedDish { get; set; }

        public ICommand DeleteCommand { get; }
        public ICommand CancelCommand { get; }

        public DeleteDishVM()
        {
            Dishes = new ObservableCollection<Food>();
            LoadDishes();

            DeleteCommand = new RelayCommands(DeleteDish);
            CancelCommand = new RelayCommands(CloseWindow);
        }

        private void LoadDishes()
        {
            try
            {
                Dishes = Sql.GetAllDishes();
                OnPropertyChanged(nameof(Dishes));

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Failed to load dishes: " + ex.Message);
            }
        }

        private void DeleteDish()
        {
            if (SelectedDish == null)
            {
                MessageBox.Show("Please select a dish to delete.");
                return;
            }

            try
            {
                Sql.DeleteDishByName(SelectedDish.Name); 

                MessageBox.Show($"Dish '{SelectedDish.Name}' was deleted.");
                Dishes.Remove(SelectedDish);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Failed to delete: " + ex.Message);
            }
        }

        private void CloseWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is View.DeleteDish)
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
