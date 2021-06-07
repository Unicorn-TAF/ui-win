using System;

namespace Unicorn.Taf.Core.Testing
{
    /// <summary>
    /// Represents object describing some defect which could be tied with fail
    /// </summary>
    [Serializable]
    public class Defect
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Defect"/> class with specified ID and type
        /// </summary>
        /// <param name="id">defect ID (for example id in BTS)</param>
        /// <param name="type">defect type</param>
        public Defect(string id, string type) : this(id, type, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Defect"/> class with specified ID and type and having some comment
        /// </summary>
        /// <param name="id">defect ID (for example id in BTS)</param>
        /// <param name="type">defect type</param>
        /// <param name="comment">comment to defect</param>
        public Defect(string id, string type, string comment)
        {
            Id = id;
            DefectType = type;
            Comment = comment;
        }

        /// <summary>
        /// Gets or sets defect id as string
        /// </summary>
        public string Id { get; protected set; }
        
        /// <summary>
        /// Gets or sets defect type as string
        /// </summary>
        public string DefectType { get; set; }

        /// <summary>
        /// Gets or sets comment to defect
        /// </summary>
        public string Comment { get; set; }
    }
}
