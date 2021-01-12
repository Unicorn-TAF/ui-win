using System;

namespace Unicorn.Taf.Core.Testing.Attributes
{
    /// <summary>
    /// Provides with ability to mark specified methods as executable after each test in suite.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class AfterTestAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AfterTestAttribute"/> class.
        /// AfterTest will be run always and will not skip following tests if failed.
        /// </summary>
        public AfterTestAttribute() : this(true, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AfterTestAttribute"/> class.
        /// AfterTest will not skip following tests if failed.
        /// </summary>
        /// <param name="runAlways">option to run AfterTest even if test was failed</param>
        public AfterTestAttribute(bool runAlways) : this(runAlways, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AfterTestAttribute"/> class.
        /// </summary>
        /// <param name="runAlways">option to run AfterTest even if test was failed</param>
        /// <param name="skipTestsOnFail">option to skip following tests if AfterTest was failed</param>
        public AfterTestAttribute(bool runAlways, bool skipTestsOnFail)
        {
            RunAlways = runAlways;
            SkipTestsOnFail = skipTestsOnFail;
        }

        /// <summary>
        /// Gets or sets a value indicating whether it's necessary 
        /// to run AfterTest even for failed tests.
        /// </summary>
        public bool RunAlways { get; }

        /// <summary>
        /// Gets or sets a value indicating whether it's necessary 
        /// to skip all following tests if current AfterTest was failed.
        /// </summary>
        public bool SkipTestsOnFail { get; }
    }
}
