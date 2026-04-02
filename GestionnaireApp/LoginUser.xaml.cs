using System.Windows;
using System.Windows.Controls;
using UserClass;
using Services; // Ensure this matches the namespace of your UserService

namespace GestionnaireApp
{
    public partial class Page1 : Page
    {
        // 1. Create an instance of the UserService to handle login logic
        private UserService _userService;

        public Page1()
        {
            InitializeComponent();
            _userService = new UserService();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = UserTextBox.Text;
            string password = PassBox.Password;

            // Basic validation
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ErrorText.Text = "Champs requis !";
                return;
            }

            // 2. Use the UserService to verify credentials
            // This checks the internal List<User> loaded from users.json
            bool isSuccess = _userService.Login(username, password);

            if (isSuccess)
            {
                // 3. Login Success! 
                // Pass the 'CurrentUser' found by the service to the MainWindow
                MainWindow main = new MainWindow(_userService.CurrentUser, _userService);
                main.Show();

                // 4. Close the Login Window container
                Window.GetWindow(this)?.Close();
            }
            else
            {
                // 5. Login Failed
                ErrorText.Text = "Utilisateur ou mot de passe incorrect.";
            }
        }

        private void GoToRegister_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the registration page (Page2)
            this.NavigationService.Navigate(new Page2());
        }
    }
}