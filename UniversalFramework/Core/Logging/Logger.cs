using System;

namespace Unicorn.Core.Logging
{
    public class Logger
    {
        private static ILogger instance;

        public static ILogger Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConsoleLogger();
                    instance.Info("Default console logger is initialized");
                }
                    
                return instance;
            }

            set
            {
                instance = value;
            }
        }
    }
}
