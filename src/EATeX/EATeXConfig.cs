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
            var assembly = Assembly.GetAssembly(GetType());
            var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = assembly.Location + ".config" };

            config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
        }

        public string Read(string key)
        {
            var foundSetting = config.AppSettings.Settings[key];
            return foundSetting != null ? foundSetting.Value : string.Empty;
        }

        public void Save()
        {
            config.Save();
        }
    }
}