using Unicorn.Taf.Core.Testing;

namespace Unicorn.Taf.Core.Reporting
{
    public interface IReporter
    {
        void Init();

        void ReportInfo(string info);

        void ReportTestStart(Test test);

        void ReportTestFinish(Test test);

        void ReportSuiteStart(TestSuite testSuite);

        void ReportSuiteFinish(TestSuite testSuite);

        void Complete();
    }
}
