using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace GestionnaireApp
{
    /// <summary>
    /// Interaction logic for CreateGame.xaml
    /// </summary>
    public partial class CreateGame : Window
    {
        public string NomApp { get; set; }
        public string CheminApp { get; set; }
        public string GenreApp { get; set; }

        public CreateGame()
        {
            InitializeComponent();
        }

        private void BrowseExe_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Applications (*.exe;*.lnk)|*.exe;*.lnk";

            if (dialog.ShowDialog() == true)
            {
                ExePathTextBox.Text = dialog.FileName;
            }
        }

        private void Confirmer_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NomTextBox.Text) ||
                string.IsNullOrEmpty(GenreTextBox.Text))
            {
                MessageBox.Show("Veuillez remplir tous les champs !");
                return;
            }

            NomApp = NomTextBox.Text;
            CheminApp = ExePathTextBox.Text;
            GenreApp = GenreTextBox.Text;
            bool isLaunchable;

            if (IsLaunchableCheckBox.IsChecked == true)
            {
                isLaunchable = true;
            }
            else
            {
                isLaunchable = false;
            }
            if (isLaunchable)
            {
                if (string.IsNullOrWhiteSpace(ExePathTextBox.Text))
                {
                    MessageBox.Show("Veuillez sélectionner un fichier (.exe ou .lnk) !", "Chemin manquant", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                CheminApp = ExePathTextBox.Text;
            }
            else
            {
                CheminApp = string.Empty;
            }

            this.DialogResult = true;
            this.Close();
        }

        private void Annuler_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}