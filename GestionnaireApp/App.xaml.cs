using System.Configuration;
using System.Data;
using System.Windows;

namespace GestionnaireApp
{
    public partial class App : Application
    {
        public static void AppliquerTheme(bool estSombre)
        {
            ResourceDictionary nouveauTheme = new ResourceDictionary();

            if (estSombre == true)
            {
                nouveauTheme.Source = new Uri("Thèmes/DarkTheme.xaml", UriKind.Relative);
            }
            else
            {
                nouveauTheme.Source = new Uri("Thèmes/LightTheme.xaml", UriKind.Relative);
            }

            var dictionnaires = Application.Current.Resources.MergedDictionaries;

            dictionnaires.Clear();

            dictionnaires.Add(nouveauTheme);
        }
    }
}