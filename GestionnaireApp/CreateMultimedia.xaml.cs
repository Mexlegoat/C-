using Microsoft.Win32;
using System.Windows;

namespace GestionnaireApp
{
    public partial class CreateMultimedia : Window
    {
        public string NomApp { get; set; }
        public string CheminApp { get; set; }
        public string TypeApp { get; set; }

        public CreateMultimedia()
        {
            InitializeComponent();
        }

        private void BrowseExe_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Applications (*.exe;*.url)|*.exe;*.url";

            if (dlg.ShowDialog() == true)
            {
                ExePathTextBox.Text = dlg.FileName;
            }
        }

        private void Confirmer_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NomTextBox.Text) ||
                string.IsNullOrEmpty(ExePathTextBox.Text) ||
                string.IsNullOrEmpty(TypeTextBox.Text))
            {
                MessageBox.Show("Veuillez remplir tous les champs !");
                return;
            }

            NomApp = NomTextBox.Text;
            CheminApp = ExePathTextBox.Text;
            TypeApp = TypeTextBox.Text;

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