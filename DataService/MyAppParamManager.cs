using Microsoft.Win32;
using System;

namespace Services
{
    public class MyAppParamManager
    {
        private const string RootPath = @"Software\GestionnaireApp";

        public static void SaveRegistryParameter(string name, string value)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(RootPath);

            if (key != null)
            {
                key.SetValue(name, value);
                key.Dispose(); 
            }
        }

        
        public static string LoadRegistryParameter(string name, string defaultValue)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(RootPath);
            string result = defaultValue;

            if (key != null)
            {
                object registryValue = key.GetValue(name);

                
                if (registryValue != null)
                {
                    result = registryValue.ToString();
                }

                key.Dispose();
            }

            return result;
        }
    }
}