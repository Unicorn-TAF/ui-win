using System;

namespace Unicorn.Taf.Core.Testing.Attributes
{
    /// <summary>
    /// Provides with ability to assign a bug to test.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class BugAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BugAttribute"/> class with specified bug reference.
        /// </summary>
        /// <param name="bug">bug id or reference</param>
        public BugAttribute(string bug) : this(bug, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BugAttribute"/> class with specified bug reference 
        /// and fail exception text part.
        /// </summary>
        /// <param name="bug">bug id or reference</param>
        /// <param name="onError">part of exception message or stacktrace test fails with</param>
        public BugAttribute(string bug, string onError) : this(bug, null, onError)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BugAttribute"/> class with specified bug reference 
        /// and fail exception type.
        /// </summary>
        /// <param name="bug">bug id or reference</param>
        /// <param name="onException">exception type test fails with</param>
        public BugAttribute(string bug, Type onException) : this(bug, onException, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BugAttribute"/> class with specified bug reference 
        /// and fail exception type and text.
        /// </summary>
        /// <param name="bug">bug id or reference</param>
        /// <param name="onException">exception type test fails with</param>
        /// <param name="onError">part of exception message or stacktrace test fails with</param>
        public BugAttribute(string bug, Type onException, string onError)
        {
            Bug = bug;
            OnException = onException;
            OnError = onError;
        }

        /// <summary>
        /// Gets or sets test bug.
        /// </summary>
        public string Bug { get; }

        /// <summary>
        /// Gets expection <see cref="Type"/>
        /// </summary>
        public Type OnException { get; }

        public string OnError { get; }
    }
}
