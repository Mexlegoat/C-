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
using Microsoft.VisualBasic;

namespace GestionnaireApp
{

    public partial class MainWindow : Window
    {
        private UserService _userService = new UserService();
        public ObservableCollection<Item> ItemsJeu { get; set; } = new();
        public ObservableCollection<Item> ItemsTravail { get; set; } = new();
        public ObservableCollection<Item> ItemsMm { get; set; } = new();
        public ObservableCollection<TypeClass> AllTypes { get; set; }
        public ObservableCollection<Item> AllItems { get; set; } = new();

        public ObservableCollection<string> Types { get; set; } = new();
        public User CurrentUser { get; set; }
        string projectPath = AppDomain.CurrentDomain.BaseDirectory;
        public MainWindow(User user, UserService service)
        {
            InitializeComponent();

            // Store the user sent from the Login page
            this.CurrentUser = user;
            this._userService = service;
            AllTypes = new ObservableCollection<TypeClass>(this.CurrentUser.UserCreatedTypes);
            // Important: Set the DataContext so the UI can see the data

            this.DataContext = this;
                        

            // On initialise l'ObservableCollection pour l'UI
            // On passe la liste de l'utilisateur en paramètre
            // (Optional) Load the user's specific lists into your collections
            LoadUserData();

        }
        private void AddTypeToItem_Click(object sender, RoutedEventArgs e)
        {
            // 1. Récupérer le bouton qui a été cliqué
            Button btn = sender as Button;

            if (btn != null)
            {
                // 2. Récupérer l'objet "Item" (Jeu, Travail ou Multimedia) lié à cette ligne
                // On utilise le DataContext pour que ça marche dans n'importe quelle ListBox
                Item applicationConcernee = btn.DataContext as Item;

                if (applicationConcernee != null)
                {
                    // 3. Trouver la ComboBox "TypeSelector" dans le même conteneur que le bouton
                    FrameworkElement parent = btn.Parent as FrameworkElement;
                    ComboBox combo = null;

                    if (parent != null)
                    {
                        // On cherche la ComboBox par son x:Name "TypeSelector" défini dans le XAML
                        combo = LogicalTreeHelper.FindLogicalNode(parent, "TypeSelector") as ComboBox;
                    }

                    if (combo != null)
                    {
                        // 4. Récupérer le type choisi dans la liste des types
                        TypeClass typeChoisi = combo.SelectedItem as TypeClass;

                        if (typeChoisi == null || typeChoisi.Nom == "(Aucun)")
                        {
                            applicationConcernee.CustomType = null;
                            MessageBox.Show($"Type retiré de {applicationConcernee.Nom}");
                        }
                        else
                        {
                            applicationConcernee.CustomType = typeChoisi;
                            MessageBox.Show($"Type '{typeChoisi.Nom}' assigné à {applicationConcernee.Nom}");
                        }
                        _userService.Save();
                        if (AppDataGrid != null)
                        {
                            AppDataGrid.Items.Refresh();
                        }
                        ListJeu.Items.Refresh();
                        ListTravail.Items.Refresh();
                        ListMm.Items.Refresh();
                    }
                }
            }
        }
        private void Supprimer_Item(object sender, RoutedEventArgs e)
        {
            // 1. On identifie l'item sur lequel on a cliqué
            Button btn = sender as Button;
            Item itemASupprimer = btn?.DataContext as Item;

            if (itemASupprimer != null)
            {
                // 2. Petite confirmation pour éviter les erreurs
                var result = MessageBox.Show($"Supprimer définitivement '{itemASupprimer.Nom}' ?",
                                             "Attention", MessageBoxButton.YesNo, MessageBoxImage.Stop);

                if (result == MessageBoxResult.Yes)
                {
                    // 3. Suppression des listes d'affichage (UI)
                    // On le retire de TOUTES les listes possibles
                    AllItems.Remove(itemASupprimer);
                    ItemsJeu.Remove(itemASupprimer);
                    ItemsTravail.Remove(itemASupprimer);
                    ItemsMm.Remove(itemASupprimer);

                    // 4. Suppression de la structure de données (JSON)
                    if (CurrentUser.Categories != null)
                    {
                        foreach (var cat in CurrentUser.Categories)
                        {
                            if (cat.Items.Contains(itemASupprimer))
                            {
                                cat.Items.Remove(itemASupprimer);
                                break;
                            }
                        }
                    }

                    // 5. Sauvegarde via le service
                    _userService.Save();

                    // 6. Rafraîchissement visuel du DataGrid
                    if (AppDataGrid != null)
                    {
                        AppDataGrid.Items.Refresh();
                    }
                }
            }
        }
        private void LoadUserData()
        {
            ItemsJeu.Clear();
            ItemsTravail.Clear();
            ItemsMm.Clear();
            AllItems.Clear();
            CurrentUser.Preferences.DefaultBrowsePath = projectPath;
            if (AllTypes == null) AllTypes = new ObservableCollection<TypeClass>();
            AllTypes.Clear();
            AllTypes.Add(new TypeClass { Nom = "(Aucun)" });
            if (CurrentUser.UserCreatedTypes != null)
            {
                foreach (var t in CurrentUser.UserCreatedTypes)
                {
                    AllTypes.Add(t);
                }
            }
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
                _userService.Save();
            }
        }
        private void Ajouter_Type(object sender, RoutedEventArgs e)
        {
            // For now, let's use a simple InputBox or a small custom Window
            string newTypeName = Microsoft.VisualBasic.Interaction.InputBox("Nom du nouveau type :", "Créer un Type", "");

            if (!string.IsNullOrWhiteSpace(newTypeName))
            {
                // 1. Create the object
                var newType = new Modeles.TypeClass { Nom = newTypeName };
                AllTypes.Add(newType);

                if (CurrentUser.UserCreatedTypes == null)
                    CurrentUser.UserCreatedTypes = new List<TypeClass>();

                CurrentUser.UserCreatedTypes.Add(newType);

                // 3. SAUVEGARDE RÉELLE
                // On demande au service de sauvegarder l'objet CurrentUser qui contient maintenant le type
                _userService.Save();

                System.Windows.MessageBox.Show($"Type '{newTypeName}' ajouté avec succès !");
            }
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Si on clique sur le fond (la Grid ou la Window), on enlève le focus/sélection
            if (e.OriginalSource is Grid || e.OriginalSource is Window || e.OriginalSource is ScrollViewer)
            {
                ListJeu.SelectedIndex = -1;
                ListTravail.SelectedIndex = -1;
                ListMm.SelectedIndex = -1;
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
                _userService.Save();
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
                _userService.Save();
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
                this.DataContext = null;
                this.DataContext = this;

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
                if(this.CurrentUser.Preferences.DoubleClickToExecute)
                {
                    if (selected is ILaunchable app)
                    {
                        if (!string.IsNullOrWhiteSpace(app.Chemin))
                        {
                            app.Launch();
                        }
                        else
                        {
                            // C'est ici que ton message d'erreur doit être !
                            MessageBox.Show($"Erreur, L'application {app.Nom} n'est pas executable!");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Erreur, Veuillez d'abord activer le double-clic dans les paramètres!");
                }

            }
        }
    }
}