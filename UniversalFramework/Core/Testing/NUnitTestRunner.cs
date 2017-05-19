using Core.Logging;
using NUnit.Framework;
using System;

namespace Core.Testing
{
    [TestFixture]
    public class NUnitTestRunner
    {
        
        

        [OneTimeSetUp]
        public static void ClassInit()
        {

        }





        [OneTimeTearDown]
        public static void ClassTearDown()
        {

        }


        protected void Execute(Action step)
        {
            Logger.Info("start step");
            //Invoke
            step();
            Logger.Info("step finished");
        }
    }
}
