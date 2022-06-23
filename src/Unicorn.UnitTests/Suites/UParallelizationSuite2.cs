using System.Diagnostics;
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
            Stopwatch sw = Stopwatch.StartNew();
            while (!ParallelSuitesHelper.Test11 && sw.ElapsedMilliseconds < 1000) ;
            Thread.Sleep(25);
            ParallelSuitesHelper.Test21 = true;
        }

        [Test]
        public void ParallelTest22()
        {
            Stopwatch sw = Stopwatch.StartNew();
            while (!ParallelSuitesHelper.Test12 && sw.ElapsedMilliseconds < 1000) ;
            Thread.Sleep(25);
            ParallelSuitesHelper.Test22 = true;
        }

        [Test]
        public void ParallelTest23()
        {
            Stopwatch sw = Stopwatch.StartNew();
            while (!ParallelSuitesHelper.Test22 && sw.ElapsedMilliseconds < 1000) ;
            Thread.Sleep(25);

            ParallelSuitesHelper.Test23 = true;
        }
    }
}
