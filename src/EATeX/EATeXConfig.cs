using System;
using System.Configuration;
using System.Reflection;

namespace EATeX
{
    public class EATeXConfig
    {
        private readonly Configuration config;

        public EATeXConfig()
        {
            var assembly = Assembly.LoadFile(AssemblyPath);
            var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = assembly.Location + ".config" };

            config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
        }

        public string Read(string key)
        {
            var keyValuePair = config.AppSettings.Settings[key];
            return keyValuePair != null ? keyValuePair.Value : string.Empty;
        }

        public void Write(string key, string value)
        {
            var keyValuePair = config.AppSettings.Settings[key];

            if (keyValuePair == null)
                return;

            keyValuePair.Value = value;
            config.Save();
        }

        private string AssemblyPath
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);

                return Uri.UnescapeDataString(uri.Path);
            }
        }
    }
}