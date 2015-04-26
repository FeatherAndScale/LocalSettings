using System.Collections.Specialized;
using System.Configuration;
using System.Threading.Tasks;
using System.Xml;
using System.Linq;
using System;
using System.Diagnostics;
using System.IO;

namespace Scale
{
    /// <summary>
    /// Gets missing AppSettings values from an optional Settings File or Environment Variables.
    /// </summary>
    public static class LocalSettings
    {
        const string FileAppSettingKey = "Scale.LocalSettings.File";

        static LocalSettings()
        {
            try
            { 
                Refresh();
            }
            catch (Exception exception)
            {
                Trace.TraceError(exception.Message);
                throw;
            }
        }

        public static void Refresh()
        {
            Settings = GetSettings();
        }

        public static NameValueCollection Settings { get; private set; }

        private static string GetFileSetting()
        {
            return ConfigurationManager.AppSettings[FileAppSettingKey];
        }

        private static NameValueCollection GetAppSettings() {
            return System.Configuration.ConfigurationManager.AppSettings;
        }

        private static NameValueCollection GetSettings()
        {
            // Get AppSettings
            var appSettings = GetAppSettings();
            var fileSettings = GetFileSettings();
            bool hasFileSettings = fileSettings.Count > 0;
            var env = System.Environment.GetEnvironmentVariables();

            // Foreach AppSetting that has no value
            var settings = new NameValueCollection(appSettings);
            foreach (string setting in appSettings)
            {
                if (string.IsNullOrEmpty(appSettings[setting]))
                {
                    // Load from Settings file
                    if (hasFileSettings && !string.IsNullOrEmpty(fileSettings[setting]))
                    {
                        settings[setting] = fileSettings[setting];
                        Trace.WriteLine(string.Format("LocalSettings: Setting \"{0}\" was set from File.", setting));
                        continue;
                    }

                    // Load from ENV
                    if (env.Contains(setting))
                    {
                        settings[setting] = env[setting] as string;
                        Trace.WriteLine(string.Format("LocalSettings: Setting \"{0}\" was set from Environment Variable.", setting));
                    }
                }
            }

            // Return
            return settings;
        }

        private static NameValueCollection GetFileSettings()
        {
            string file = GetFileSetting();
            if (string.IsNullOrEmpty(file)) return new NameValueCollection();
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);
            
            if (!File.Exists(filePath))
            {
                // file not found, return empty collection.
                Trace.TraceInformation("AppSettings[{0}] specifies \"{1}\" as the settings file, but it was not found at \"{2}\".", 
                    FileAppSettingKey, file, filePath);
                return new NameValueCollection();
            }

            using (var reader = XmlReader.Create(filePath))
            {
                var settings = new NameValueCollection();
                while (!reader.EOF)
                {
                    if (!reader.Read()) break;
                    if (reader.Name == "add") settings.Add(reader.GetAttribute("key"), reader.GetAttribute("value"));
                }

                reader.Close();
                return settings;
            }
        }

        //public static void LoadSettings()
        //{
        //    string file = GetFileSetting();
        //    if (string.IsNullOrEmpty(file)) return;

        //    // http://msdn.microsoft.com/en-us/library/system.configuration.appsettingssection(v=vs.110).aspx

        //    // Get the current configuration associated 
        //    // with the application.
        //    var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        //    // Associate the auxiliary with the default 
        //    // configuration file. 
        //    var appSettings = config.AppSettings;
        //    appSettings.File = file;

        //    // Save the configuration file.
        //    config.Save(ConfigurationSaveMode.Modified);

        //    // Force a reload in memory of the  
        //    // changed section.
        //    ConfigurationManager.RefreshSection("appSettings");
        //}

        //public static void LoadSettings(NameValueCollection settings)
        //{
        //    System.Configuration.ConfigurationManager.AppSettings.Add(settings);
        //}

    }
}
