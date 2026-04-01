using Microsoft.Win32;
using System;

namespace Services
{
    public class MyAppParamManager
    {
        private const string RootPath = @"Software\GestionnaireApp";

        // Saves a parameter to the Registry
        public static void SaveRegistryParameter(string name, string value)
        {
            // Create or open the subkey
            RegistryKey key = Registry.CurrentUser.CreateSubKey(RootPath);

            // Check if the key was successfully created/opened
            if (key != null)
            {
                key.SetValue(name, value);
                key.Dispose(); // Manually close the key since we aren't using 'using'
            }
        }

        // Loads a parameter from the Registry
        public static string LoadRegistryParameter(string name, string defaultValue)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(RootPath);
            string result = defaultValue;

            if (key != null)
            {
                object registryValue = key.GetValue(name);

                // If the value exists in the registry, use it
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