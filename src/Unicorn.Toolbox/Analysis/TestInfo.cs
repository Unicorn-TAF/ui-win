using System.Collections.Generic;
using System.Text;

namespace Unicorn.Toolbox.Analysis
{
    public struct TestInfo
    {
        private List<string> categories;
        private string name;
        private string author;

        public TestInfo(string testName)
        {
            this.name = testName;
            this.categories = new List<string>();
            this.author = string.Empty;
        }

        public TestInfo(string testName, string author, IEnumerable<string> categories)
        {
            this.name = testName;
            this.author = author;
            this.categories = new List<string>(categories);
        }

        public string Name => this.name;

        public string Author => this.author;

        public List<string> Categories => this.categories;

        public override string ToString()
        {
            StringBuilder info = new StringBuilder();
            info.AppendLine($"Test: {this.Name}")
                .AppendLine($"\tAuthor: {this.Author}")
                .AppendLine($"\tCategories: {string.Join(",", this.Categories)}");

            return info.ToString();
        }
    }
}
