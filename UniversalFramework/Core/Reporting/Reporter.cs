using System;

namespace Unicorn.Core.Reporting
{
    public class Reporter
    {

        private static IReporter _instance;
        public static IReporter Instance
        {
            get
            {
                if (_instance == null)
                    throw new NullReferenceException("Reporter instance is not created.");

                return _instance;
            }
            set
            {
                _instance = value;
            }
        }


    }
}
