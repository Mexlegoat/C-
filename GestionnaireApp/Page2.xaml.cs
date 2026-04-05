using System.Windows;
using System.Windows.Controls;
using UserClass;
using Services;

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

            bool success = _userService.Register(username, password);

            if (success)
            {
                _userService.CurrentUser.Categories = new List<CategoryClass.Categorie>
                {
                    new CategoryClass.Categorie { Nom = "Jeux", Items = new List<ItemClass.Item>() },
                    new CategoryClass.Categorie { Nom = "Travail", Items = new List<ItemClass.Item>() },
                    new CategoryClass.Categorie { Nom = "Multimedia", Items = new List<ItemClass.Item>() }
                };

                _userService.Save();

                MainWindow main = new MainWindow(_userService.CurrentUser, _userService);
                main.Show();

                Window.GetWindow(this)?.Close();
            }
            else
            {
                RegErrorText.Text = "Ce nom d'utilisateur est déjà pris.";
            }
        }

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
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