using System.Collections.Specialized;
using System.Configuration;
using System.Threading.Tasks;
using System.Xml;

namespace Scale
{
    public static class LocalSettings
    {
        const string FileAppSettingKey = "Scale.LocalSettings.File";

        public static void LoadSettings()
        {
            string file = GetFileSetting();
            if (string.IsNullOrEmpty(file)) return;

            // http://msdn.microsoft.com/en-us/library/system.configuration.appsettingssection(v=vs.110).aspx

            // Get the current configuration associated 
            // with the application.
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            // Associate the auxiliary with the default 
            // configuration file. 
            var appSettings = config.AppSettings;
            appSettings.File = file;

            // Save the configuration file.
            config.Save(ConfigurationSaveMode.Modified);

            // Force a reload in memory of the  
            // changed section.
            ConfigurationManager.RefreshSection("appSettings");
        }

        public static void LoadSettings(NameValueCollection settings)
        {
            System.Configuration.ConfigurationManager.AppSettings.Add(settings);
        }

        private static string GetFileSetting()
        {
            return ConfigurationManager.AppSettings[FileAppSettingKey];
        }

        private async static Task<NameValueCollection> GetSettings()
        {
            string file = GetFileSetting();
            if (string.IsNullOrEmpty(file))
                throw new System.InvalidOperationException(string.Format("Could not find appSetting \"{0}\"",
                    FileAppSettingKey));

            var reader = XmlReader.Create(file, new XmlReaderSettings { Async = true });

            var settings = new NameValueCollection();
            while (!reader.EOF)
            {
                if (!await reader.ReadAsync()) break;
                if (reader.Name == "add") settings.Add(reader.GetAttribute("key"), reader.GetAttribute("value"));
            }

            return settings;
        }
    }
}
