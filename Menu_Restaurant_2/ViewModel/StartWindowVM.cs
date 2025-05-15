using Menu_Restaurant_2.Commands;
using Menu_Restaurant_2.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Menu_Restaurant_2.View;
using Menu_Restaurant_2.Model;
using Menu_Restaurant.Json;
using System.Reflection;


namespace Menu_Restaurant_2.ViewModel
{
    public class StartWindowVM
    {
        public ICommand LogInCommand { get; }
        public ICommand SignUpCommand { get; }
        public ICommand ContinueWithoutAccountCommand { get; }

        private RestaurantDbContext _context;
        public event Action NavigateToMenuPage;
        public StartWindowVM()
        {
            _context = new RestaurantDbContext();
            LogInCommand = new RelayCommands(OnLogIn);
            SignUpCommand = new RelayCommands(OnSignUp);
            ContinueWithoutAccountCommand = new RelayCommands(OnContinueWithoutAccount);
        }
        private void OnLogIn()
        {
            var loginDialog = new LoginDialogBox("Enter your email:", "Enter your password:");
            if (loginDialog.ShowDialog() == true)
            {
                string enteredEmail = loginDialog.UserInput1?.Trim().ToLower();
                string enteredParola = loginDialog.UserInput2?.Trim();

                var person = _context.Person
                    .SingleOrDefault(p => p.Email.ToLower() == enteredEmail && p.Parola == enteredParola);

                if (person != null)
                {
                    var userService = new UserService();
                    userService.ClearUser();
                    userService.SaveUser(person);

                    NavigateToMenuPage?.Invoke(); 
                }
                else
                {
                    MessageBox.Show("Email or password is incorrect.");
                }
            }
        }


        private void OnSignUp()
        {
            bool isValid = false;

            while (!isValid)
            {
              
                var signUpDialog = new SignUpDialogBox(
                    "Enter a username:",
                    "Enter account type (customer/employee):",
                    "Enter your email:",
                    "Enter a password:"
                );

                if (signUpDialog.ShowDialog() == true)
                {
                    string enteredName = signUpDialog.UserInput1?.Trim();
                    string enteredType = signUpDialog.UserInput2?.Trim().ToLower();
                    string enteredEmail = signUpDialog.UserInput3?.Trim();
                    string enteredParola = signUpDialog.UserInput4?.Trim();

                    if (string.IsNullOrWhiteSpace(enteredName))
                    {
                        MessageBox.Show("Username cannot be empty.");
                        continue;
                    }

                    if (enteredType != "customer" && enteredType != "employee")
                    {
                        MessageBox.Show("Please enter a valid account type: customer or employee.");
                        continue;
                    }

                    // Verificare unicitate username
                    var existingUser = _context.Person.SingleOrDefault(p => p.Name == enteredName);
                    if (existingUser == null)
                    {
                        var newUser = new Person
                        {
                            Name = enteredName,
                            Type = enteredType,
                            Email = enteredEmail,
                            Parola = enteredParola
                        };

                        _context.Person.Add(newUser);
                        _context.SaveChanges();

                        var userService = new UserService();
                        userService.ClearUser();
                        userService.SaveUser(newUser);

                        MessageBox.Show($"Account created successfully: {enteredName} ({enteredType})");
                        isValid = true;
                        NavigateToMenuPage?.Invoke();  
                    }
                    else
                    {
                        MessageBox.Show($"Username '{enteredName}' already exists. Please choose another.");
                    }
                }
            }
        }


        private void OnContinueWithoutAccount()
        {
            UserService userService = new UserService();
            userService.ClearUser();
            NavigateToMenuPage?.Invoke();
        }

    }
}
