using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using ItemClass;
using System.Globalization;
using System.Windows.Data;
using System.Drawing;

namespace GestionnaireApp
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Item> ItemsJeu { get; set; } = new();
        public ObservableCollection<Item> ItemsTravail { get; set; } = new();
        public ObservableCollection<Item> ItemsMm { get; set; } = new();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void CreerJeu(object sender, RoutedEventArgs e)
        {
            var window = new CreateGame();
            if (window.ShowDialog() == true)
            {
                ItemsJeu.Add(new Item
                {
                    Nom = window.NomApp,
                    Chemin = window.CheminApp,
                    Genre = window.GenreApp,
                    IconPath = window.CheminApp
                });
            }
        }

        private void CreerTravail(object sender, RoutedEventArgs e)
        {
            var window = new CreateWork();
            if (window.ShowDialog() == true)
            {
                ItemsTravail.Add(new Item
                {
                    Nom = window.NomApp,
                    Chemin = window.CheminApp,
                    Genre = window.LangageApp,
                    IconPath = window.CheminApp
                });
            }
        }

        private void CreerMm(object sender, RoutedEventArgs e)
        {
            var window = new CreateMultimedia();
            if (window.ShowDialog() == true)
            {
                ItemsMm.Add(new Item
                {
                    Nom = window.NomApp,
                    Chemin = window.CheminApp,
                    Genre = window.TypeApp,
                    IconPath = window.CheminApp
                });
            }
        }

        private void LaunchApp(object sender, MouseButtonEventArgs e)
        {
            if ((sender as System.Windows.Controls.ListBox)?.SelectedItem is Item app)
            {
                try
                {
                    Process.Start(app.Chemin);
                }
                catch
                {
                    MessageBox.Show("Impossible de lancer l'application.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}