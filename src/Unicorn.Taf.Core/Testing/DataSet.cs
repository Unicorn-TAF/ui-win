using System.Collections.Generic;

namespace Unicorn.Taf.Core.Testing
{
    /// <summary>
    /// Represents set of test or suite data.<para/>
    /// Contains set name, array of objects representing set of data itself
    /// </summary>
    public class DataSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataSet"/> class with specified name and data objects
        /// </summary>
        /// <param name="name">set name</param>
        /// <param name="parameters">array of objects</param>
        public DataSet(string name, params object[] parameters)
        {
            Name = name;
            Parameters = new List<object>(parameters);
        }

        /// <summary>
        /// Gets or sets <see cref="DataSet"/> name
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Gets array of data set objects
        /// </summary>
        public List<object> Parameters { get; }
    }
}
