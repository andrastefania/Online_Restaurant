using System.Windows;

namespace Menu_Restaurant_2.View
{
    public partial class SignUpDialogBox : Window
    {
        // Properties to get the text entered in the TextBoxes
        public string UserInput1 => InputTextBox1.Text.Trim();
        public string UserInput2 => InputTextBox2.Text.Trim();
        public string UserInput3 => InputTextBox3.Text.Trim();
        public string UserInput4 => InputTextBox4.Text.Trim();

        public SignUpDialogBox(string message1, string message2, string message3, string message4)
        {
            InitializeComponent();
            MessageText1.Text = message1;
            MessageText2.Text = message2;
            MessageText3.Text = message3;
            MessageText4.Text = message4;
            InputTextBox1.Focus(); // focus pe primul input
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(InputTextBox1.Text) &&
                !string.IsNullOrWhiteSpace(InputTextBox2.Text) &&
                !string.IsNullOrWhiteSpace(InputTextBox3.Text) &&
                !string.IsNullOrWhiteSpace(InputTextBox4.Text))
            {
                this.DialogResult = true; 
                this.Close();
            }
            else
            {
                MessageBox.Show("Please fill in both fields before continuing.");
            }
        }
    }
}
