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
using Modeles;
using UserClass;
using CategoryClass;
using Services;
using System.Windows.Controls;

namespace GestionnaireApp
{
    public partial class MainWindow : Window
    {
        private UserService _userService = new UserService();
        public ObservableCollection<Item> ItemsJeu { get; set; } = new();
        public ObservableCollection<Item> ItemsTravail { get; set; } = new();
        public ObservableCollection<Item> ItemsMm { get; set; } = new();
        public ObservableCollection<Item> AllItems { get; set; } = new();

        public ObservableCollection<string> Types { get; set; } = new();
        public User CurrentUser { get; set; }
        string projectPath = AppDomain.CurrentDomain.BaseDirectory;
        public MainWindow(User user)
        {
            InitializeComponent();
            
            // Store the user sent from the Login page
            this.CurrentUser = user;

            // Important: Set the DataContext so the UI can see the data
            this.DataContext = this;

            // (Optional) Load the user's specific lists into your collections
            LoadUserData();
        }
        private void LoadUserData()
        {
            ItemsJeu.Clear();
            ItemsTravail.Clear();
            ItemsMm.Clear();
            AllItems.Clear(); // Clear the master list
            CurrentUser.Preferences.DefaultBrowsePath = projectPath;

            if (CurrentUser.Categories != null)
            {
                foreach (var cat in CurrentUser.Categories)
                {
                    foreach (var item in cat.Items)
                    {
                        // Add to specific list
                        if (cat.Nom == "Jeux") ItemsJeu.Add(item);
                        else if (cat.Nom == "Travail") ItemsTravail.Add(item);
                        else if (cat.Nom == "Multimedia") ItemsMm.Add(item);

                        // Also add to the "Everything" list
                        AllItems.Add(item);
                    }
                }
            }
            SyncIdCompteur();
        }
        private void CreerJeu(object sender, RoutedEventArgs e)
        {
            var window = new CreateGame();
            if (window.ShowDialog() == true)
            {
                // 1. Create the object
                var nouveauJeu = new Jeu
                {
                    Nom = window.NomApp,
                    Chemin = window.CheminApp,
                    Genre = window.GenreApp,
                    IconPath = window.CheminApp
                };

                // 2. Add to the UI list (so it shows up immediately)
                nouveauJeu.InitializeNewItem();
                ItemsJeu.Add(nouveauJeu);
                AllItems.Add(nouveauJeu);
                // 3. Add to the User's Data Structure
                var catJeux = CurrentUser.Categories.FirstOrDefault(c => c.Nom == "Jeux");
                if (catJeux == null)
                {
                    catJeux = new Categorie { Nom = "Jeux" };
                    CurrentUser.Categories.Add(catJeux);
                }
                catJeux.Items.Add(nouveauJeu);

                // 4. Save to the JSON file
                // (Assuming you have a reference to your UserService or call DataService directly)
                Services.DataService.Save("users.json", new List<User> { CurrentUser });
            }
        }
        private void Ajouter_Type(object sender, RoutedEventArgs e)
        {
            // For now, let's use a simple InputBox or a small custom Window
            string newTypeName = Microsoft.VisualBasic.Interaction.InputBox("Nom du nouveau type :", "Créer un Type", "");

            if (!string.IsNullOrWhiteSpace(newTypeName))
            {
                // 1. Create the object
                var newType = new Modeles.TypeClass { Type = newTypeName };

                // 2. Add it to the User's collection
                CurrentUser.UserCreatedTypes.Add(newType);

                // 3. Save to JSON via UserService
                _userService.Save();

                System.Windows.MessageBox.Show($"Type '{newTypeName}' ajouté avec succès !");
            }
        }
        private void CreerTravail(object sender, RoutedEventArgs e)
        {
            var window = new CreateWork();
            if (window.ShowDialog() == true)
            {
                // 1. Create the object (Using the properties from CreateWork.xaml.cs)
                var nouveauTravail = new Travail
                {
                    Nom = window.NomApp,
                    Chemin = window.CheminApp,
                    Langage = window.LangageApp, // Specific to Travail
                    IconPath = window.CheminApp
                };

                // 2. Add to the UI list
                nouveauTravail.InitializeNewItem();

                ItemsTravail.Add(nouveauTravail);
                AllItems.Add(nouveauTravail); // Add to DataGrid lis

                // 3. Add to the User's Data Structure
                var catTravail = CurrentUser.Categories.FirstOrDefault(c => c.Nom == "Travail");
                if (catTravail == null)
                {
                    catTravail = new Categorie { Nom = "Travail", Items = new List<Item>() };
                    CurrentUser.Categories.Add(catTravail);
                }
                catTravail.Items.Add(nouveauTravail);

                // 4. Save to the JSON file
                // Note: Make sure you save the whole list of users if your DataService expects a List<User>
                Services.DataService.Save("users.json", new List<User> { CurrentUser });
            }
        }

        private void CreerMm(object sender, RoutedEventArgs e)
        {
            var window = new CreateMultimedia();
            if (window.ShowDialog() == true)
            {
                // 1. Create the object
                var nouveauMm = new Multimedia
                {
                    Nom = window.NomApp,
                    Chemin = window.CheminApp,
                    Genre = window.TypeApp, // Note: You used 'Genre' in your class for Multimedia Type
                    IconPath = window.CheminApp
                };

                // 2. Add to the UI list
                nouveauMm.InitializeNewItem();

                AllItems.Add(nouveauMm); // Add to DataGrid lis
                ItemsMm.Add(nouveauMm);

                // 3. Add to the User's Data Structure
                var catMm = CurrentUser.Categories.FirstOrDefault(c => c.Nom == "Multimedia");
                if (catMm == null)
                {
                    catMm = new Categorie { Nom = "Multimedia", Items = new List<Item>() };
                    CurrentUser.Categories.Add(catMm);
                }
                catMm.Items.Add(nouveauMm);

                // 4. Save to the JSON file
                Services.DataService.Save("users.json", new List<User> { CurrentUser });
            }
        }
        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox list = sender as ListBox;

            if (list != null)
            {
                // Set the DataGrid's source to the same source as the clicked ListBox
                AppDataGrid.ItemsSource = list.ItemsSource;
            }
        }
        private void Ouvrir_Param(object sender, RoutedEventArgs e)
        {
            // 1. Create the window and pass the CurrentUser
            var settingsWin = new SettingsWindow(CurrentUser);

            // 2. Show the window as a dialog (blocks interaction with main window)
            if (settingsWin.ShowDialog() == true)
            {
                // 3. If they clicked "Save", persist the changes to the JSON file
                _userService.Save();

                // 4. Force the UI to refresh (in case they changed 'Show Type')
                LoadUserData();

                MessageBox.Show("Paramètres enregistrés !");
            }
        }
        private void SyncIdCompteur()
        {
            int maxId = 0;

            // We check all 3 collections to find the absolute highest ID currently in use
            foreach (var item in AllItems)
            {
                if (item.Id > maxId) maxId = item.Id;
            }

            // Tell the Item class to start the next ID from maxId + 1
            // Note: This requires a static method in your Item class (see below)
            Item.SetNextId(maxId + 1);
        }
        private void LaunchApp(object sender, MouseButtonEventArgs e)
        {
            ListBox list = sender as ListBox;

            if (list != null)
            {
                object selected = list.SelectedItem;

                if (selected is ILaunchable app)
                {
                    app.Launch();
                }
            }
        }
    }
}