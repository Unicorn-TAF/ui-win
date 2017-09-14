using Unicorn.Core.Testing.Tests;

namespace Unicorn.Core.Reporting
{
    public interface IReporter
    {
        void Init();


        void Report(string info);


        void ReportTest(Test test);


        void ReportSuite(TestSuite testSuite);


        void Complete();

    }
}
