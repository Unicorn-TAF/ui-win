namespace Unicorn.Taf.Core.Logging
{
    /// <summary>
    /// Entry point of framework logger.
    /// </summary>
    public static class Logger
    {
        private static ILogger instance;

        /// <summary>
        /// Gets or sets framework logger instance (default logger is <see cref="DefaultLogger"/>).
        /// </summary>
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

        /// <summary>
        /// Gets or sets value to filter out log messages by severity (default is Debug).
        /// </summary>
        public static LogLevel Level { get; set; } = LogLevel.Debug;
    }
}
