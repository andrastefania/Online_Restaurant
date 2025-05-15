using System.Windows;

namespace Menu_Restaurant_2.View
{
    public partial class LoginDialogBox : Window
    {
        
        public string UserInput1 => InputTextBox1.Text;
        public string UserInput2 => InputTextBox2.Text;

        
        public LoginDialogBox(string message1,string message2)
        {
            InitializeComponent();
            MessageText1.Text = message1;
            MessageText2.Text = message2;
            InputTextBox1.Focus();
        }

       
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(InputTextBox1.Text) && !string.IsNullOrWhiteSpace(InputTextBox2.Text))
            {
                this.DialogResult = true;  
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter a value before continuing.");
            }
        }
    }
}
