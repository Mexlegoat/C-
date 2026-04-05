using System.Configuration;
using System.Data;
using System.Windows;

namespace GestionnaireApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static void AppliquerTheme(bool estSombre)
        {
            // 1. Déclarer le dictionnaire de ressources
            ResourceDictionary nouveauTheme = new ResourceDictionary();

            // 2. Choisir le fichier selon le booléen (If au lieu de ?)
            if (estSombre == true)
            {
                nouveauTheme.Source = new Uri("Thèmes/DarkTheme.xaml", UriKind.Relative);
            }
            else
            {
                nouveauTheme.Source = new Uri("Thèmes/LightTheme.xaml", UriKind.Relative);
            }

            // 3. Accéder aux dictionnaires fusionnés de l'application
            var dictionnaires = Application.Current.Resources.MergedDictionaries;

            // 4. Nettoyer les thèmes actuels pour éviter les conflits
            dictionnaires.Clear();

            // 5. Ajouter le nouveau thème
            dictionnaires.Add(nouveauTheme);
        }
    }


}
