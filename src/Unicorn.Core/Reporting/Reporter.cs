using System;

namespace Unicorn.Core.Reporting
{
    public static class Reporter
    {
        private static IReporter instance;

        public static IReporter Instance
        {
            get
            {
                CheckForNullInstance();
                return instance;
            }

            set
            {
                instance = value;
            }
        }

        private static void CheckForNullInstance()
        {
            if (instance == null)
            {
                throw new ArgumentException("Reporter instance is not created.");
            }
        }
    }
}
