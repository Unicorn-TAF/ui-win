namespace Unicorn.Core.Logging
{
    public static class Logger
    {
        private static ILogger instance;

        public static ILogger Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DefaultLogger();
                    instance.Log(LogLevel.Info, "Default console logger is initialized");
                }
                    
                return instance;
            }

            set
            {
                instance = value;
            }
        }

        public static LogLevel Level { get; set; } = LogLevel.Debug;
    }
}
