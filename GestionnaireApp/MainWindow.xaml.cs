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
            this.CurrentUser = user;
            this._userService = service;
            AllTypes = new ObservableCollection<TypeClass>(this.CurrentUser.UserCreatedTypes);
            this.DataContext = this;
            if (user.Preferences.IsDarkMode == true)
            {
                App.AppliquerTheme(true);
            }
            else
            {
                App.AppliquerTheme(false);
            }
            LoadUserData();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filtre = SearchTextBox.Text.ToLower();
            if (string.IsNullOrWhiteSpace(filtre))
            {
                ListJeu.ItemsSource = ItemsJeu;
                ListTravail.ItemsSource = ItemsTravail;
                ListMm.ItemsSource = ItemsMm;
                AppDataGrid.ItemsSource = AllItems;
            }
            else
            {
                ListJeu.ItemsSource = ItemsJeu.Where(a => AppliquerFiltre(a, filtre)).ToList();
                ListTravail.ItemsSource = ItemsTravail.Where(a => AppliquerFiltre(a, filtre)).ToList();
                ListMm.ItemsSource = ItemsMm.Where(a => AppliquerFiltre(a, filtre)).ToList();
                AppDataGrid.ItemsSource = AllItems.Where(a => AppliquerFiltre(a, filtre)).ToList();
            }
        }

        private bool AppliquerFiltre(dynamic app, string texte)
        {
            int choix = CurrentUser.Preferences.SearchType;
            if (choix == 0)
            {
                if (app.Nom != null) return app.Nom.ToLower().Contains(texte);
                return false;
            }
            if (choix == 1)
            {
                if (app.CustomType != null)
                {
                    if (app.CustomType.Nom != null) return app.CustomType.Nom.ToLower().Contains(texte);
                }
                return false;
            }
            if (choix == 2)
            {
                var propertyGenre = app.GetType().GetProperty("Genre");
                if (propertyGenre != null)
                {
                    var valeur = propertyGenre.GetValue(app);
                    if (valeur != null) return valeur.ToString().ToLower().Contains(texte);
                }
                var propertyLang = app.GetType().GetProperty("Langage");
                if (propertyLang != null)
                {
                    var valeurLang = propertyLang.GetValue(app);
                    if (valeurLang != null) return valeurLang.ToString().ToLower().Contains(texte);
                }
            }
            return false;
        }

        private void AddTypeToItem_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                Item applicationConcernee = btn.DataContext as Item;
                if (applicationConcernee != null)
                {
                    FrameworkElement parent = btn.Parent as FrameworkElement;
                    ComboBox combo = null;
                    if (parent != null)
                    {
                        combo = LogicalTreeHelper.FindLogicalNode(parent, "TypeSelector") as ComboBox;
                    }
                    if (combo != null)
                    {
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
            Button btn = sender as Button;
            Item itemASupprimer = btn?.DataContext as Item;
            if (itemASupprimer != null)
            {
                var result = MessageBox.Show($"Supprimer définitivement '{itemASupprimer.Nom}' ?",
                                             "Attention", MessageBoxButton.YesNo, MessageBoxImage.Stop);
                if (result == MessageBoxResult.Yes)
                {
                    AllItems.Remove(itemASupprimer);
                    ItemsJeu.Remove(itemASupprimer);
                    ItemsTravail.Remove(itemASupprimer);
                    ItemsMm.Remove(itemASupprimer);
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
                    _userService.Save();
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
                        if (cat.Nom == "Jeux") ItemsJeu.Add(item);
                        else if (cat.Nom == "Travail") ItemsTravail.Add(item);
                        else if (cat.Nom == "Multimedia") ItemsMm.Add(item);
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
                var nouveauJeu = new Jeu
                {
                    Nom = window.NomApp,
                    Chemin = window.CheminApp,
                    Genre = window.GenreApp,
                    IconPath = window.CheminApp
                };
                nouveauJeu.InitializeNewItem();
                ItemsJeu.Add(nouveauJeu);
                AllItems.Add(nouveauJeu);
                var catJeux = CurrentUser.Categories.FirstOrDefault(c => c.Nom == "Jeux");
                if (catJeux == null)
                {
                    catJeux = new Categorie { Nom = "Jeux" };
                    CurrentUser.Categories.Add(catJeux);
                }
                catJeux.Items.Add(nouveauJeu);
                _userService.Save();
            }
        }

        private void Ajouter_Type(object sender, RoutedEventArgs e)
        {
            string newTypeName = Microsoft.VisualBasic.Interaction.InputBox("Nom du nouveau type :", "Créer un Type", "");
            if (!string.IsNullOrWhiteSpace(newTypeName))
            {
                var newType = new Modeles.TypeClass { Nom = newTypeName };
                AllTypes.Add(newType);
                if (CurrentUser.UserCreatedTypes == null)
                    CurrentUser.UserCreatedTypes = new List<TypeClass>();
                CurrentUser.UserCreatedTypes.Add(newType);
                _userService.Save();
                System.Windows.MessageBox.Show($"Type '{newTypeName}' ajouté avec succès !");
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
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
                var nouveauTravail = new Travail
                {
                    Nom = window.NomApp,
                    Chemin = window.CheminApp,
                    Langage = window.LangageApp,
                    IconPath = window.CheminApp
                };
                nouveauTravail.InitializeNewItem();
                ItemsTravail.Add(nouveauTravail);
                AllItems.Add(nouveauTravail);
                var catTravail = CurrentUser.Categories.FirstOrDefault(c => c.Nom == "Travail");
                if (catTravail == null)
                {
                    catTravail = new Categorie { Nom = "Travail", Items = new List<Item>() };
                    CurrentUser.Categories.Add(catTravail);
                }
                catTravail.Items.Add(nouveauTravail);
                _userService.Save();
            }
        }

        private void CreerMm(object sender, RoutedEventArgs e)
        {
            var window = new CreateMultimedia();
            if (window.ShowDialog() == true)
            {
                var nouveauMm = new Multimedia
                {
                    Nom = window.NomApp,
                    Chemin = window.CheminApp,
                    Genre = window.TypeApp,
                    IconPath = window.CheminApp
                };
                nouveauMm.InitializeNewItem();
                AllItems.Add(nouveauMm);
                ItemsMm.Add(nouveauMm);
                var catMm = CurrentUser.Categories.FirstOrDefault(c => c.Nom == "Multimedia");
                if (catMm == null)
                {
                    catMm = new Categorie { Nom = "Multimedia", Items = new List<Item>() };
                    CurrentUser.Categories.Add(catMm);
                }
                catMm.Items.Add(nouveauMm);
                _userService.Save();
            }
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox list = sender as ListBox;
            if (list != null)
            {
                AppDataGrid.ItemsSource = list.ItemsSource;
            }
        }

        private void Ouvrir_Param(object sender, RoutedEventArgs e)
        {
            var settingsWin = new SettingsWindow(CurrentUser);
            if (settingsWin.ShowDialog() == true)
            {
                _userService.Save();
                this.DataContext = null;
                this.DataContext = this;
                LoadUserData();
                MessageBox.Show("Paramètres enregistrés !");
            }
        }

        private void SyncIdCompteur()
        {
            int maxId = 0;
            foreach (var item in AllItems)
            {
                if (item.Id > maxId) maxId = item.Id;
            }
            Item.SetNextId(maxId + 1);
        }

        private void LaunchApp(object sender, MouseButtonEventArgs e)
        {
            ListBox list = sender as ListBox;
            if (list != null)
            {
                object selected = list.SelectedItem;
                if (this.CurrentUser.Preferences.DoubleClickToExecute)
                {
                    if (selected is ILaunchable app)
                    {
                        if (!string.IsNullOrWhiteSpace(app.Chemin))
                        {
                            app.Launch();
                        }
                        else
                        {
                            MessageBox.Show($"Erreur, L'application {app.Nom} n'est pas executable!");
                        }
                    }
                }
            }
        }
    }
}