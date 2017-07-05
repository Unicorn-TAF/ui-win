using System;

namespace Unicorn.Core.Logging
{
    public class Logger
    {
        private static ILogger _instance;
        public static ILogger Instance
        {
            get
            {
                if (_instance == null)
                    throw new NullReferenceException("Logger instance is not created.");

                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

    }
}
