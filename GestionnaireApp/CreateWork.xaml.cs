using Microsoft.Win32;
using System.Windows;

namespace GestionnaireApp
{
    public partial class CreateWork : Window
    {
        public string NomApp { get; set; }
        public string CheminApp { get; set; }
        public string LangageApp { get; set; }

        public CreateWork()
        {
            InitializeComponent();
        }

        private void BrowseExe_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Applications (*.exe;*.bat;*.cmd)|*.exe;*.bat;*.cmd";

            if (dlg.ShowDialog() == true)
            {
                ExePathTextBox.Text = dlg.FileName;
            }
        }

        private void Confirmer_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NomTextBox.Text) ||
                string.IsNullOrEmpty(ExePathTextBox.Text) ||
                string.IsNullOrEmpty(LangageTextBox.Text))
            {
                MessageBox.Show("Veuillez remplir tous les champs !");
                return;
            }

            NomApp = NomTextBox.Text;
            CheminApp = ExePathTextBox.Text;
            LangageApp = LangageTextBox.Text;

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