using System;
using System.Linq;
using System.Text;
using System.Xml;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Testing;

namespace Unicorn.Taf.Core.Utility
{
    public class TrxCreator
    {
        private const string Adapter = "Microsoft.VisualStudio.TestTools.TestTypes.Unit.UnitTestAdapter, Microsoft.VisualStudio.QualityTools.Tips.UnitTest.Adapter, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
        private const string DtFormat = "yyyy-MM-ddTHH:mm:ss.fffffffK";

        private readonly XmlDocument trx;
        private readonly XmlElement xTestDefinitions;
        private readonly XmlElement xTestLists;
        private readonly XmlElement xTestEntries;
        private readonly XmlElement xResults;
        private readonly string machineName = Environment.MachineName;

        private readonly int error = 0;
        private readonly int timeout = 0;
        private readonly int aborted = 0;
        private readonly int passedButRunAborted = 0;
        private readonly int notRunnable = 0;
        private readonly int disconnected = 0;
        private readonly int warning = 0;
        private readonly int inProgress = 0;
        private readonly int pending = 0;

        private int total = 0;
        private int executed = 0;
        private int passed = 0;
        private int failed = 0;
        private int inconclusive = 0;
        private int notExecuted = 0;
        private int completed = 0;

        public TrxCreator()
        {
            this.trx = new XmlDocument();
            var xmlDeclaration = trx.CreateXmlDeclaration("1.0", "UTF-8", null);
            var root = trx.DocumentElement;
            trx.InsertBefore(xmlDeclaration, root);

            xTestDefinitions = trx.CreateElement(string.Empty, "TestDefinitions", string.Empty);
            xTestLists = trx.CreateElement(string.Empty, "TestLists", string.Empty);
            xTestEntries = trx.CreateElement(string.Empty, "TestEntries", string.Empty);
            xResults = trx.CreateElement(string.Empty, "Results", string.Empty);
        }

        public void GenerateTrxFile(LaunchOutcome outcome, string trxPath)
        {
            if (!outcome.SuitesOutcomes.Any())
            {
                return;
            }

            try
            {
                var xTestRun = GetTestRun(outcome.StartTime);

                foreach (var suiteOutcome in outcome.SuitesOutcomes)
                {
                    FillTrxWithSuiteData(suiteOutcome);
                }

                var xResultSummary = GetResultSummary();

                xTestRun.AppendChild(xResultSummary);
                xTestRun.AppendChild(xTestDefinitions);
                xTestRun.AppendChild(xTestLists);
                xTestRun.AppendChild(xTestEntries);
                xTestRun.AppendChild(xResults);

                trx.AppendChild(xTestRun);

                using (var writer = new XmlTextWriter(trxPath, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;
                    trx.Save(writer);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("TRX test results file generation failed. " + e);
            }
        }

        private XmlElement GetTestRun(DateTime startTime)
        {
            var xTestRun = trx.CreateElement(string.Empty, "TestRun", string.Empty);

            var xmlns = trx.CreateAttribute("xmlns");
            xmlns.Value = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010";

            var runId = trx.CreateAttribute("id");
            runId.Value = Guid.NewGuid().ToString();

            var runName = trx.CreateAttribute("name");
            runName.Value = "Tests run: " + DateTime.Now;

            var runUser = trx.CreateAttribute("runUser");
            runUser.Value = Environment.UserName;

            #region "Times"

            var xTimes = trx.CreateElement(string.Empty, "Times", string.Empty);

            var creation = trx.CreateAttribute("creation");
            creation.Value = startTime.AddMilliseconds(-20).ToString(DtFormat);

            var queuing = trx.CreateAttribute("queuing");
            queuing.Value = startTime.AddMilliseconds(-10).ToString(DtFormat);

            var start = trx.CreateAttribute("start");
            start.Value = startTime.ToString(DtFormat);

            var finish = trx.CreateAttribute("finish");
            finish.Value = DateTime.Now.ToString(DtFormat);

            xTimes.Attributes.Append(creation);
            xTimes.Attributes.Append(queuing);
            xTimes.Attributes.Append(start);
            xTimes.Attributes.Append(finish);

            #endregion

            xTestRun.Attributes.Append(runId);
            xTestRun.Attributes.Append(runName);
            xTestRun.Attributes.Append(runUser);
            xTestRun.Attributes.Append(xmlns);
            xTestRun.AppendChild(xTimes);

            return xTestRun;
        }

        private XmlElement GetUnitTest(TestOutcome outcome, string executionIdValue)
        {
            var xUnitTest = trx.CreateElement(string.Empty, "UnitTest", string.Empty);

            var name = trx.CreateAttribute("name");
            name.Value = outcome.FullMethodName;

            var storage = trx.CreateAttribute("storage");
            storage.Value = string.Empty; ////TODO: Path.GetFileName(outcome.testMethodInfo.DeclaringType.Assembly.Location));

            var id = trx.CreateAttribute("id");
            id.Value = outcome.Id.ToString();

            xUnitTest.Attributes.Append(name);
            xUnitTest.Attributes.Append(storage);
            xUnitTest.Attributes.Append(id);

            // Description.
            var xDescription = trx.CreateElement(string.Empty, "Description", string.Empty);
            xDescription.InnerText = outcome.Title;

            // Execution.
            var xExecution = trx.CreateElement(string.Empty, "Execution", string.Empty);

            var executionId = trx.CreateAttribute("id");
            executionId.Value = executionIdValue;

            xExecution.Attributes.Append(executionId);

            // TestMethod.
            var xTestMethod = trx.CreateElement(string.Empty, "TestMethod", string.Empty);

            var codeBase = trx.CreateAttribute("codeBase");
            codeBase.Value = string.Empty; ////TODO: testMethodInfo.DeclaringType.Assembly.CodeBase.Replace("file:///", string.Empty);

            var adapterTypeName = trx.CreateAttribute("adapterTypeName");
            adapterTypeName.Value = Adapter;

            var className = trx.CreateAttribute("className");
            className.Value = string.Empty; ////TODO: testMethodInfo.DeclaringType.AssemblyQualifiedName;

            var aName = trx.CreateAttribute("name");
            aName.Value = outcome.FullMethodName;

            xTestMethod.Attributes.Append(codeBase);
            xTestMethod.Attributes.Append(adapterTypeName);
            xTestMethod.Attributes.Append(className);
            xTestMethod.Attributes.Append(aName);

            xUnitTest.AppendChild(xDescription);
            xUnitTest.AppendChild(xExecution);
            xUnitTest.AppendChild(xTestMethod);

            return xUnitTest;
        }

        private void FillTrxWithSuiteData(SuiteOutcome outcome)
        {
            var xTestList = GetTestList(outcome);
            xTestLists.AppendChild(xTestList);

            foreach (var testOutcome in outcome.TestsOutcomes)
            {
                total++;

                if (outcome.Result.Equals(Status.Skipped))
                {
                    inconclusive++;
                    notExecuted++;
                    continue;
                }

                executed++;
                completed++;

                if (outcome.Result.Equals(Status.Passed))
                {
                    passed++;
                }
                else
                {
                    failed++;
                }

                var executionIdValue = AdapterUtilities.GuidFromString(testOutcome.StartTime.ToString(DtFormat)).ToString();

                var xUnitTest = GetUnitTest(testOutcome, executionIdValue);
                var xTestEntry = GetTestEntry(testOutcome, executionIdValue);
                var xUnitTestResult = GetUnitTestResult(testOutcome, executionIdValue);

                xTestDefinitions.AppendChild(xUnitTest);
                xTestEntries.AppendChild(xTestEntry);
                xResults.AppendChild(xUnitTestResult);
            }
        }

        private XmlElement GetTestList(SuiteOutcome outcome)
        {
            var xTestList = trx.CreateElement(string.Empty, "TestList", string.Empty);

            var tlName = trx.CreateAttribute("name");
            tlName.Value = outcome.Name;

            var tlId = trx.CreateAttribute("id");
            tlId.Value = outcome.Id.ToString();

            xTestList.Attributes.Append(tlName);
            xTestList.Attributes.Append(tlId);

            return xTestList;
        }

        private XmlElement GetUnitTestResult(TestOutcome outcome, string executionIdValue)
        {
            var xUnitTestResult = trx.CreateElement(string.Empty, "UnitTestResult", string.Empty);

            var executionId = trx.CreateAttribute("executionId");
            executionId.Value = executionIdValue;

            var testId = trx.CreateAttribute("testId");
            testId.Value = outcome.Id.ToString();

            var testName = trx.CreateAttribute("testName");
            testName.Value = outcome.FullMethodName;

            var computerName = trx.CreateAttribute("computerName");
            computerName.Value = machineName;

            var duration = trx.CreateAttribute("duration");
            duration.Value = string.Format("{0:hh\\:mm\\:ss\\.fffffff}", outcome.ExecutionTime);

            var startTime = trx.CreateAttribute("startTime");
            startTime.Value = outcome.StartTime.ToString(DtFormat);

            var endTime = trx.CreateAttribute("endTime");
            endTime.Value = outcome.StartTime.Add(outcome.ExecutionTime).ToString(DtFormat);

            var testType = trx.CreateAttribute("testType");
            testType.Value = "13cdc9d9-ddb5-4fa4-a97d-d965ccfc6d4b";

            var aOutcome = trx.CreateAttribute("outcome");

            switch (outcome.Result)
            {
                case Status.Passed:
                    aOutcome.Value = "Passed";
                    break;
                case Status.Failed:
                    aOutcome.Value = "Failed";
                    break;
                case Status.Skipped:
                    aOutcome.Value = "Inconclusive";
                    break;
            }

            var testListId = trx.CreateAttribute("testListId");
            testListId.Value = outcome.ParentId.ToString();

            xUnitTestResult.Attributes.Append(executionId);
            xUnitTestResult.Attributes.Append(testId);
            xUnitTestResult.Attributes.Append(testName);
            xUnitTestResult.Attributes.Append(computerName);
            xUnitTestResult.Attributes.Append(duration);
            xUnitTestResult.Attributes.Append(startTime);
            xUnitTestResult.Attributes.Append(endTime);
            xUnitTestResult.Attributes.Append(testType);
            xUnitTestResult.Attributes.Append(aOutcome);
            xUnitTestResult.Attributes.Append(testListId);

            var output = trx.CreateElement(string.Empty, "Output", string.Empty);
            var stdOut = trx.CreateElement(string.Empty, "StdOut", string.Empty);

            #region "FailInfo"

            if (outcome.Result.Equals(Status.Failed))
            {
                var xErrorInfo = trx.CreateElement(string.Empty, "ErrorInfo", string.Empty);
                var xMessage = trx.CreateElement(string.Empty, "Message", string.Empty);
                var xStackTrace = trx.CreateElement(string.Empty, "StackTrace", string.Empty);

                xMessage.InnerText = outcome.Exception.Message;
                xStackTrace.InnerText = outcome.Exception.StackTrace;
                stdOut.InnerText = outcome.Output;

                xErrorInfo.AppendChild(xMessage);
                xErrorInfo.AppendChild(xStackTrace);
                output.AppendChild(xErrorInfo);
            }

            #endregion

            output.AppendChild(stdOut);

            xUnitTestResult.AppendChild(output);

            return xUnitTestResult;
        }

        private XmlElement GetTestEntry(TestOutcome outcome, string executionIdValue)
        {
            var xTestEntry = trx.CreateElement(string.Empty, "TestEntry", string.Empty);

            var testId = trx.CreateAttribute("testId");
            testId.Value = outcome.Id.ToString();

            var executionId = trx.CreateAttribute("executionId");
            executionId.Value = executionIdValue;

            var testListId = trx.CreateAttribute("testListId");
            testListId.Value = outcome.ParentId.ToString();

            xTestEntry.Attributes.Append(testId);
            xTestEntry.Attributes.Append(executionId);
            xTestEntry.Attributes.Append(testListId);

            return xTestEntry;
        }

        private XmlElement GetResultSummary()
        {
            var xResultSummary = trx.CreateElement(string.Empty, "ResultSummary", string.Empty);
            var xCounters = trx.CreateElement(string.Empty, "Counters", string.Empty);

            var cTotal = trx.CreateAttribute("total");
            cTotal.Value = total.ToString();

            var cExecuted = trx.CreateAttribute("executed");
            cExecuted.Value = executed.ToString();

            var cPassed = trx.CreateAttribute("passed");
            cPassed.Value = passed.ToString();

            var cError = trx.CreateAttribute("error");
            cError.Value = error.ToString();

            var cFailed = trx.CreateAttribute("failed");
            cFailed.Value = failed.ToString();

            var cTimeout = trx.CreateAttribute("timeout");
            cTimeout.Value = timeout.ToString();

            var cAborted = trx.CreateAttribute("aborted");
            cAborted.Value = aborted.ToString();

            var cInconclusive = trx.CreateAttribute("inconclusive");
            cInconclusive.Value = inconclusive.ToString();

            var cPassedButRunAborted = trx.CreateAttribute("passedButRunAborted");
            cPassedButRunAborted.Value = passedButRunAborted.ToString();

            var cNotRunnable = trx.CreateAttribute("notRunnable");
            cNotRunnable.Value = notRunnable.ToString();

            var cNotExecuted = trx.CreateAttribute("notExecuted");
            cNotExecuted.Value = notExecuted.ToString();

            var cDisconnected = trx.CreateAttribute("disconnected");
            cDisconnected.Value = disconnected.ToString();

            var cWarning = trx.CreateAttribute("warning");
            cWarning.Value = warning.ToString();

            var cCompleted = trx.CreateAttribute("completed");
            cCompleted.Value = completed.ToString();

            var cInProgress = trx.CreateAttribute("inProgress");
            cInProgress.Value = inProgress.ToString();

            var cPending = trx.CreateAttribute("pending");
            cPending.Value = pending.ToString();

            xCounters.Attributes.Append(cTotal);
            xCounters.Attributes.Append(cExecuted);
            xCounters.Attributes.Append(cPassed);
            xCounters.Attributes.Append(cError);
            xCounters.Attributes.Append(cFailed);
            xCounters.Attributes.Append(cTimeout);
            xCounters.Attributes.Append(cAborted);
            xCounters.Attributes.Append(cInconclusive);
            xCounters.Attributes.Append(cPassedButRunAborted);
            xCounters.Attributes.Append(cNotRunnable);
            xCounters.Attributes.Append(cNotExecuted);
            xCounters.Attributes.Append(cDisconnected);
            xCounters.Attributes.Append(cWarning);
            xCounters.Attributes.Append(cCompleted);
            xCounters.Attributes.Append(cInProgress);
            xCounters.Attributes.Append(cPending);

            xResultSummary.AppendChild(xCounters);

            return xResultSummary;
        }
    }
}
