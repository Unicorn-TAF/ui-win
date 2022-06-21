using System.Threading;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.UnitTests.Suites
{
    [Suite("Suite that tests parallelization by suite 2")]
    [Tag(Tag.SuiteParallelizaton)]
    public class UParallelizationSuite2 : TestSuite
    {
        [Test]
        public void ParallelTest21()
        {
            while (!ParallelSuitesHelper.Test11);
            Thread.Sleep(25);
            ParallelSuitesHelper.Test21 = true;
        }

        [Test]
        public void ParallelTest22()
        {
            while (!ParallelSuitesHelper.Test12);
            Thread.Sleep(25);
            ParallelSuitesHelper.Test22 = true;
        }

        [Test]
        public void ParallelTest23()
        {
            while (!ParallelSuitesHelper.Test22);
            Thread.Sleep(25);

            ParallelSuitesHelper.Test23 = true;
        }
    }
}
