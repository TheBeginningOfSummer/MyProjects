using MyToolkit;

namespace IPFSVideo.Service
{
    public class ConfigurationService
    {
        private static readonly object locker = new();
        private static ConfigurationService? instance;
        public static ConfigurationService Instance
        {
            get
            {
                if (instance == null)
                    lock (locker)
                        instance ??= new ConfigurationService();
                return instance;
            }
        }

        public KeyValueLoader Config;

        private ConfigurationService()
        {
            Config = new KeyValueLoader("Config.json", "Config");
        }

        
    }
}
