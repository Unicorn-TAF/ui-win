using System.Threading;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.UnitTests.Suites
{
    [Suite("Suite that tests parallelization by suite 1")]
    [Tag(Tag.SuiteParallelizaton)]
    public class UParallelizationSuite1 : TestSuite
    {
        [Test]
        public void ParallelTest11()
        {
            ParallelSuitesHelper.Test11 = true;
        }

        [Test]
        public void ParallelTest12()
        {
            while (!ParallelSuitesHelper.Test21);
            Thread.Sleep(25);
            ParallelSuitesHelper.Test12 = true;
        }

        [Test]
        public void ParallelTest13()
        {
            while (!ParallelSuitesHelper.Test23);
            Thread.Sleep(1);
        }
    }
}
