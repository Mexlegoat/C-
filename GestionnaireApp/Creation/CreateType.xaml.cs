using Microsoft.Win32;
using Modeles;
using Services;
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
using UserClass;

namespace GestionnaireApp
{
    /// <summary>
    /// Interaction logic for CreateGame.xaml
    /// </summary>
    public partial class CreateType : Window
    {
        public string Type { get; set; }

        public CreateType()
        {
            InitializeComponent();
        }


        private void Confirmer_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TypeTextBox.Text))
            {
                MessageBox.Show("Veuillez remplir tous les champs !");
                return;
            }

            Type = TypeTextBox.Text;
            
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