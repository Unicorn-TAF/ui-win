using System.Collections.Generic;

namespace Unicorn.Taf.Core.Testing
{
    public class DataSet
    {
        public DataSet(string name, params object[] parameters)
        {
            this.Name = name;
            this.Parameters = new List<object>(parameters);
        }

        public string Name { get; protected set; }

        public List<object> Parameters { get; }
    }
}
