﻿
namespace Unicorn.Core.Testing
{
    public class Defect
    {
        public const string ToInvestigate = "To Investigate";
        public const string ProductBug = "Product Bug";
        public const string SystemIssue = "System Issue";

        public Defect(string id) : this(id, ToInvestigate, string.Empty)
        {
        }

        public Defect(string id, string type) : this(id, type, string.Empty)
        {
        }

        public Defect(string id, string type, string comment)
        {
            this.Id = id;
            this.Type = type;
            this.Comment = comment;
        }

        public string Id { get; protected set; }
        
        public string Type { get; set; }

        public string Comment { get; set; }
    }
}
