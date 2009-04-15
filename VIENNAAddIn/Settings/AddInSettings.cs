using System;
using Microsoft.Win32;
using VIENNAAddIn.Exceptions;

namespace VIENNAAddIn.Settings
{
    ///<summary>
    /// Global Add-In settings.
    ///</summary>
    public static class AddInSettings
    {
        ///<summary>
        ///</summary>
        public const string AddInName = "VIENNAAddIn";

        ///<summary>
        ///</summary>
        public static string CommonXSDPath { get; private set; }

        /// <summary>
        /// Retrieves settings from the windows registry.
        /// </summary>
        public static void LoadRegistryEntries()
        {
            try
            {
                // the addin might be installed for everyone or just for one user
                const string viennaAddInRegistryKey = "SOFTWARE\\VIENNAAddIn";
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(viennaAddInRegistryKey) ??
                                          Registry.LocalMachine.OpenSubKey(viennaAddInRegistryKey);
                string homeDirectory = registryKey.LoadRegistryEntry("homedir");
                CommonXSDPath = homeDirectory + registryKey.LoadRegistryEntry("commonxsd");
            }
            catch (RegistryAccessException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new RegistryAccessException(e.Message, e);
            }
        }
    }

    internal static class RegistryKeyExtensions
    {
        internal static string LoadRegistryEntry(this RegistryKey registryKey, string entryName)
        {
            var value = (String) registryKey.GetValue(entryName);
            if (value == null || value.Trim().Length == 0)
            {
                throw new RegistryAccessException(String.Format("Registry entry '{0}' of key '{1}' not found", entryName,
                                                                registryKey.Name));
            }
            return value;
        }
    }
}