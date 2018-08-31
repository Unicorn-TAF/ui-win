using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Toolbox.Analysis
{
    public struct TestInfo
    {
        public const string NoCategory = "<CATEGORY NOT SPECIFIED>";

        private List<string> categories;
        private string name;
        private string author;

        public TestInfo(string testName, string author, IEnumerable<string> categories)
        {
            this.name = testName;
            this.author = author;
            this.categories = new List<string>(categories);

            if (!this.categories.Any())
            {
                this.categories.Add(NoCategory);
            }
        }

        public string Name => this.name;

        public string Author => this.author;

        public List<string> Categories => this.categories;
    }
}
