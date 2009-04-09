using System;
using System.IO;
using Microsoft.Win32;
using VIENNAAddIn.Exceptions;

namespace VIENNAAddIn.Settings
{
    ///<summary>
    /// Global Add-In settings.
    ///</summary>
    public static class AddInSettings
    {
        // If this boolean field is set to true, the VIENNAAddIn is built in GIEM mode.
        // This means, that all menu entries are named as GIEM instead of VIENNAAddIn.
        private const bool buildGIEM = false;
        private const String VIENNAAddIn_ADDIN_REGISTRY_KEY = "SOFTWARE\\VIENNAAddIn";

        /// <summary>
        /// Return the caption of the Add-In
        /// </summary>
        /// <returns></returns>
        public static string AddInCaption
        {
            get { return buildGIEM ? "GIEM" : "VIENNAAddIn"; }
        }

        private static string ImagesPath { get; set; }

        ///<summary>
        ///</summary>
        public static string BPELTemplatePath { get; private set; }

        ///<summary>
        ///</summary>
        public static string CCLibraryFilePath { get; private set; }

        ///<summary>
        ///</summary>
        public static string MDGFilePath { get; private set; }

        private static string HomeDirectory { get; set; }

        /// <summary>
        /// Retrieves settings from the windows registry.
        /// </summary>
        internal static void LoadRegistryEntries()
        {
            try
            {
                // the addin might be installed for everyone or just for one user
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(VIENNAAddIn_ADDIN_REGISTRY_KEY) ??
                                          Registry.LocalMachine.OpenSubKey(VIENNAAddIn_ADDIN_REGISTRY_KEY);
                HomeDirectory = registryKey.LoadRegistryEntry("homedir");
                MDGFilePath = HomeDirectory + registryKey.LoadRegistryEntry(buildGIEM ? "mdgfilegiem" : "mdgfile");
                CCLibraryFilePath = HomeDirectory + registryKey.LoadRegistryEntry("cclibrary");
                BPELTemplatePath = HomeDirectory + registryKey.LoadRegistryEntry("bpelTemplate");
                ImagesPath = HomeDirectory + registryKey.LoadRegistryEntry("images");
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

        /// <summary>
        /// Get MDG file path from registry and read it
        /// </summary>
        /// <returns>MDG in string</returns>
        internal static string LoadMDGFile()
        {
            using (TextReader reader = new StreamReader(MDGFilePath))
            {
                return reader.ReadToEnd();
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