using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingWithAutomation.Helpers
{
    public class ConfigHelper
    {
        private static JObject configData;

        static ConfigHelper()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectRootDirectory = Directory.GetParent(baseDirectory).Parent.Parent.Parent.FullName;
            string configPath = Path.Combine(projectRootDirectory, "envConfig.json");

            string jsonContent = File.ReadAllText(configPath);
            configData = JObject.Parse(jsonContent);
        }

        public static string GetConfigValue(string key)
        {
            return configData[key]?.ToString();
        }
    }

}
