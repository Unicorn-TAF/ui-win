using System;

namespace Unicorn.Core.Reporting
{
    public class Reporter
    {
        private static IReporter instance;

        public static IReporter Instance
        {
            get
            {
                if (instance == null)
                {
                    throw new NullReferenceException("Reporter instance is not created.");
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
