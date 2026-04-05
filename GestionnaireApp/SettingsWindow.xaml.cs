using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UserClass;
namespace GestionnaireApp
{
    public partial class SettingsWindow : Window
    {
        private User _user;

        public SettingsWindow(User user)
        {
            InitializeComponent();
            _user = user;
            LoadSettings();
        }

        private void LoadSettings()
        {
            RadioSombre.IsChecked = _user.Preferences.IsDarkMode;
            RadioClair.IsChecked = !_user.Preferences.IsDarkMode;
            CheckShowType.IsChecked = _user.Preferences.ShowType;
            CheckShowGenre.IsChecked = _user.Preferences.ShowGenre;
            RadioExecActive.IsChecked = _user.Preferences.DoubleClickToExecute;
            PathTextBox.Text = _user.Preferences.DefaultBrowsePath;
            if (_user.Preferences.SearchType == 0)
            {
                RadioNom.IsChecked = true;
            }
            else if (_user.Preferences.SearchType == 1)
            {
                RadioType.IsChecked = true;
            }
            else if (_user.Preferences.SearchType == 2)
            {
                RadioGenre.IsChecked = true;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // --- 1. Update the User Object (for the JSON/Session) ---

            // Check Dark Mode
            if (RadioSombre.IsChecked == true)
            {
                _user.Preferences.IsDarkMode = true;
            }
            else
            {
                _user.Preferences.IsDarkMode = false;
            }

            // Check Show Type
            if (CheckShowType.IsChecked == true)
            {
                _user.Preferences.ShowType = true;
            }
            else
            {
                _user.Preferences.ShowType = false;
            }
            if (CheckShowGenre.IsChecked == true)
            {
                _user.Preferences.ShowGenre = true;
            }
            else
            {
                _user.Preferences.ShowGenre = false;
            }
            // Check Double Click Execution
            if (RadioExecActive.IsChecked == true)
            {
                _user.Preferences.DoubleClickToExecute = true;
            }
            else
            {
                _user.Preferences.DoubleClickToExecute = false;
            }

            // Save the Browse Path from the TextBox
            _user.Preferences.DefaultBrowsePath = PathTextBox.Text;

            if (RadioSombre.IsChecked == true)
            {
                _user.Preferences.IsDarkMode = true;
                App.AppliquerTheme(true);
            }
            else
            {
                _user.Preferences.IsDarkMode = false;
                App.AppliquerTheme(false);
            }
            if (RadioNom.IsChecked == true) _user.Preferences.SearchType = 0;
            else if (RadioType.IsChecked == true) _user.Preferences.SearchType = 1;
            else if (RadioGenre.IsChecked == true) _user.Preferences.SearchType = 2;
            // --- 2. Update the Registry (Requirement d) ---
            // We save these so Windows remembers them even if the JSON is moved
            Services.MyAppParamManager.SaveRegistryParameter("IsDarkMode", _user.Preferences.IsDarkMode.ToString());
            Services.MyAppParamManager.SaveRegistryParameter("ShowType", _user.Preferences.ShowType.ToString());
            Services.MyAppParamManager.SaveRegistryParameter("SearchType", _user.Preferences.SearchType.ToString());
            Services.MyAppParamManager.SaveRegistryParameter("ShowGenre", _user.Preferences.ShowGenre.ToString());
            Services.MyAppParamManager.SaveRegistryParameter("StoragePath", PathTextBox.Text);

            
            // --- 3. Close the Modal Window ---
            this.DialogResult = true;
            this.Close();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {

            string folderPath = PathTextBox.Text;

            if (string.IsNullOrEmpty(folderPath))
            {
                folderPath = AppDomain.CurrentDomain.BaseDirectory;
            }

            // 3. Check if the folder actually exists on the computer
            if (Directory.Exists(folderPath))
            {
                // 4. This command tells Windows: "Open a real Explorer window at this path"
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = folderPath,
                    FileName = "explorer.exe"
                };

                Process.Start(startInfo);
            }
            else
            {
                // Optional: If the path is broken, tell the user
                System.Windows.MessageBox.Show("Le chemin n'existe pas : " + folderPath);
            }
        }
    }
}
