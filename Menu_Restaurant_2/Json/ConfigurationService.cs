using System;
using System.IO;
using Newtonsoft.Json;
using Menu_Restaurant_2.Model;
using System.Windows;

namespace Menu_Restaurant_2.Json
{
    public static class ConfigurationService
    {
        private const string FilePath = "config.json";

        public static AppConfig LoadConfig()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    throw new FileNotFoundException("Fișierul config.json nu a fost găsit.");
                }

                var json = File.ReadAllText(FilePath);
                return JsonConvert.DeserializeObject<AppConfig>(json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la citirea fișierului de configurare: " + ex.Message);
                return new AppConfig();
            }
        }
    }
}
