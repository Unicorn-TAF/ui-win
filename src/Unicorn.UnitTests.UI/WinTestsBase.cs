﻿using NUnit.Framework;
using Unicorn.UI.Win;

#pragma warning disable S2187 // TestCases should contain tests
namespace Unicorn.UnitTests
{
    [TestFixture]
    public class WinTestsBase
    {
        public static WinScreenshotTaker Screenshot { get; set; }

        [OneTimeSetUp]
        public static void ClassInit()
        {
            Screenshot = new WinScreenshotTaker();
        }

        [OneTimeTearDown]
        public static void ClassCleanup()
        {
            Screenshot = null;
        }
    }
}
#pragma warning restore S2187 // TestCases should contain tests
