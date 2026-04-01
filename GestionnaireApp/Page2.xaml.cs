using System.Windows;
using System.Windows.Controls;
using UserClass;
using Services; // Assuming your UserService is here

namespace GestionnaireApp
{
    public partial class Page2 : Page
    {
        private UserService _userService;

        public Page2()
        {
            InitializeComponent();
            _userService = new UserService();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string username = NewUserTextBox.Text;
            string password = NewPassBox.Password;
            string confirm = ConfirmPassBox.Password;

            // 1. Basic Validation
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                RegErrorText.Text = "Tous les champs sont requis.";
                return;
            }

            if (password != confirm)
            {
                RegErrorText.Text = "Les mots de passe ne correspondent pas.";
                return;
            }

            // 2. Try to register via the UserService
            // This calls your Register method which checks if the name is taken
            bool success = _userService.Register(username, password);

            if (success)
            {
                // 3. Create default categories for the new user 
                // So they aren't empty when MainWindow loads
                _userService.CurrentUser.Categories = new List<CategoryClass.Categorie>
                {
                    new CategoryClass.Categorie { Nom = "Jeux", Items = new List<ItemClass.Item>() },
                    new CategoryClass.Categorie { Nom = "Travail", Items = new List<ItemClass.Item>() },
                    new CategoryClass.Categorie { Nom = "Multimedia", Items = new List<ItemClass.Item>() }
                };

                // Save the user again with their new empty categories
                _userService.Save();

                // 4. Success! Open MainWindow
                MainWindow main = new MainWindow(_userService.CurrentUser);
                main.Show();

                // Close the Login Window
                Window.GetWindow(this)?.Close();
            }
            else
            {
                RegErrorText.Text = "Ce nom d'utilisateur est déjà pris.";
            }
        }

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            // Simply go back to Page1
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
            else
            {
                this.NavigationService.Navigate(new Page1());
            }
        }
    }
}