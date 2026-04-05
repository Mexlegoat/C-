using System.Windows;
using System.Windows.Controls;
using UserClass;
using Services;

namespace GestionnaireApp
{
    public partial class Page1 : Page
    {
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

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ErrorText.Text = "Champs requis !";
                return;
            }

            bool isSuccess = _userService.Login(username, password);

            if (isSuccess)
            {
                MainWindow main = new MainWindow(_userService.CurrentUser, _userService);
                main.Show();

                Window.GetWindow(this)?.Close();
            }
            else
            {
                ErrorText.Text = "Utilisateur ou mot de passe incorrect.";
            }
        }

        private void GoToRegister_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Page2());
        }
    }
}