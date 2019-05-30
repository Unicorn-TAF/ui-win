using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Unicorn.Taf.Core.Testing;

namespace Unicorn.Taf.Core.Engine
{
    internal class TrxCreator
    {
        private const string Adapter = "Microsoft.VisualStudio.TestTools.TestTypes.Unit.UnitTestAdapter, Microsoft.VisualStudio.QualityTools.Tips.UnitTest.Adapter, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
        private const string DtFormat = "yyyy-MM-ddTHH:mm:ss.fffffffK";

        private int total = 0;
        private int executed = 0;
        private int passed = 0;
        private int error = 0;
        private int failed = 0;
        private int timeout = 0;
        private int aborted = 0;
        private int inconclusive = 0;
        private int passedButRunAborted = 0;
        private int notRunnable = 0;
        private int notExecuted = 0;
        private int disconnected = 0;
        private int warning = 0;
        private int completed = 0;
        private int inProgress = 0;
        private int pending = 0;

        private XmlDocument trx;

        public TrxCreator()
        {
            this.trx = new XmlDocument();
            var xmlDeclaration = trx.CreateXmlDeclaration("1.0", "UTF-8", null);
            var root = trx.DocumentElement;
            trx.InsertBefore(xmlDeclaration, root);
        }

        public void GenerateTrxFile(LaunchOutcome outcome)
        {
            if (!outcome.SuitesOutcomes.Any())
            {
                return;
            }

            try
            {
                var machineName = Environment.MachineName;

                var testRun = GenerateTestRun(outcome.StartTime);

                var testDefinitions = trx.CreateElement(string.Empty, "TestDefinitions", string.Empty);
                var testLists = trx.CreateElement(string.Empty, "TestLists", string.Empty);
                var testEntries = trx.CreateElement(string.Empty, "TestEntries", string.Empty);
                var results = trx.CreateElement(string.Empty, "Results", string.Empty);

                foreach (var suiteOutcome in outcome.SuitesOutcomes)
                {
                    FillTrxWithSuiteData(suiteOutcome, ref testDefinitions, ref testLists, ref testEntries, ref results);
                }

                #region ResultSummary

                var resultSummary = trx.CreateElement(string.Empty, "ResultSummary", string.Empty);
                var counters = trx.CreateElement(string.Empty, "Counters", string.Empty);

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

                counters.Attributes.Append(cTotal);
                counters.Attributes.Append(cExecuted);
                counters.Attributes.Append(cPassed);
                counters.Attributes.Append(cError);
                counters.Attributes.Append(cFailed);
                counters.Attributes.Append(cTimeout);
                counters.Attributes.Append(cAborted);
                counters.Attributes.Append(cInconclusive);
                counters.Attributes.Append(cPassedButRunAborted);
                counters.Attributes.Append(cNotRunnable);
                counters.Attributes.Append(cNotExecuted);
                counters.Attributes.Append(cDisconnected);
                counters.Attributes.Append(cWarning);
                counters.Attributes.Append(cCompleted);
                counters.Attributes.Append(cInProgress);
                counters.Attributes.Append(cPending);

                resultSummary.AppendChild(counters);
                testRun.AppendChild(resultSummary);

                #endregion

                testRun.AppendChild(testDefinitions);
                testRun.AppendChild(testLists);
                testRun.AppendChild(testEntries);
                testRun.AppendChild(results);

                trx.AppendChild(testRun);

                var trxPath = Path.Combine(Global.PathTestsDir, "TestData", $"_testresults_{machineName}.trx");

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

        private XmlElement GenerateTestRun(DateTime startTime)
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

        private void FillTrxWithSuiteData(SuiteOutcome outcome, ref XmlElement testDefinitions, ref XmlElement testLists, ref XmlElement testEntries, ref XmlElement results)
        {
            var xTestList = trx.CreateElement(string.Empty, "TestList", string.Empty);

            var tlName = trx.CreateAttribute("name");
            tlName.Value = outcome. Name;

            var tlId = trx.CreateAttribute("id");
            tlId.Value = outcome.Id.ToString();

            xTestList.Attributes.Append(tlName);
            xTestList.Attributes.Append(tlId);

            testLists.AppendChild(xTestList);

            foreach (var test in outcome.TestsOutcomes)
            {
                total++;

                // now skipped reporting of skipped tests
                if (outcome.Result.Equals(Status.Skipped))
                {
                    inconclusive++;
                    continue;
                }

                executed++;

                if (outcome.Result.Equals(Status.Passed))
                {
                    passed++;
                }
                else
                {
                    failed++;
                }

                var unitTest = GetUnitTest(test);
                var testEntry = GetTestEntry(test, suite);
                var unitTestResult = GetUnitTestResult(test, suite);

                testDefinitions.AppendChild(unitTest);
                testEntries.AppendChild(testEntry);
                results.AppendChild(unitTestResult);
            }
        }

        private XmlElement GetUnitTestResult(Test test, TestSuite suite)
        {
            var unitTestResult = trx.CreateElement(string.Empty, "UnitTestResult", string.Empty);

            var executionId = trx.CreateAttribute("executionId");
            executionId.Value = GetGuid(test.Outcome.StartTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK"));

            var testId = trx.CreateAttribute("testId");
            testId.Value = test.Id.ToString();

            var testName = trx.CreateAttribute("testName");
            testName.Value = test.FullTestName;

            var computerName = trx.CreateAttribute("computerName");
            computerName.Value = Environment.MachineName;

            var duration = trx.CreateAttribute("duration");
            duration.Value = string.Format("{0:hh\\:mm\\:ss\\.fffffff}", test.Outcome.ExecutionTime);

            var startTime = trx.CreateAttribute("startTime");
            startTime.Value = test.Outcome.StartTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK");

            var endTime = trx.CreateAttribute("endTime");
            endTime.Value = test.Outcome.StartTime.Add(test.Outcome.ExecutionTime).ToString("yyyy-MM-ddTHH:mm:ss.fffffffK");

            var testType = trx.CreateAttribute("testType");
            testType.Value = "13cdc9d9-ddb5-4fa4-a97d-d965ccfc6d4b";

            var outcome = trx.CreateAttribute("outcome");

            switch (test.Outcome.Result)
            {
                case Result.PASSED:
                    outcome.Value = "Passed";
                    break;
                case Result.FAILED:
                    outcome.Value = "Failed";
                    break;
                case Result.SKIPPED:
                    outcome.Value = "Inconclusive";
                    break;
            }

            var testListId = trx.CreateAttribute("testListId");
            testListId.Value = suite.Id.ToString();

            unitTestResult.Attributes.Append(executionId);
            unitTestResult.Attributes.Append(testId);
            unitTestResult.Attributes.Append(testName);
            unitTestResult.Attributes.Append(computerName);
            unitTestResult.Attributes.Append(duration);
            unitTestResult.Attributes.Append(startTime);
            unitTestResult.Attributes.Append(endTime);
            unitTestResult.Attributes.Append(testType);
            unitTestResult.Attributes.Append(outcome);
            unitTestResult.Attributes.Append(testListId);

            var output = trx.CreateElement(string.Empty, "Output", string.Empty);
            var stdOut = trx.CreateElement(string.Empty, "StdOut", string.Empty);



            if (test.Outcome.Result.Equals(Result.FAILED))
            {
                var errorInfo = trx.CreateElement(string.Empty, "ErrorInfo", string.Empty);

                var message = trx.CreateElement(string.Empty, "Message", string.Empty);
                var stackTrace = trx.CreateElement(string.Empty, "StackTrace", string.Empty);

                try
                {
                    var bug = test.Outcome.Bugs.Any() ? "" : "\nBug: " + test.Outcome.Bugs[0];
                    var screenshot = Path.Combine(Global.PathDropFolder, "Screenshots", test.Outcome.Screenshot);
                    message.InnerText = string.Format("{0}\n\nScreenshot: {1}{2}", test.Outcome.Exception.Message, screenshot, bug);
                    stackTrace.InnerText = test.Outcome.Exception.StackTrace;
                    stdOut.InnerText = test.ToString();
                }
                catch (Exception e)
                {
                    message.InnerText = "Something goes wrong while getting error message...";
                    stackTrace.InnerText = "Please analyse logs and screenshots to fix this reporting issue. \n" + e;
                }

                errorInfo.AppendChild(message);
                errorInfo.AppendChild(stackTrace);
                output.AppendChild(errorInfo);
            }

            output.AppendChild(stdOut);

            unitTestResult.AppendChild(output);

            return unitTestResult;
        }

        private XmlElement GetTestEntry(TestOutcome outcome)
        {
            var xTestEntry = trx.CreateElement(string.Empty, "TestEntry", string.Empty);

            var testId = trx.CreateAttribute("testId");
            testId.Value = outcome.Id.ToString();

            var executionId = trx.CreateAttribute("executionId");
            executionId.Value = GetGuid(outcome.Outcome.StartTime.ToString(DtFormat));

            var testListId = trx.CreateAttribute("testListId");
            testListId.Value = outcome.ParentId.ToString();

            xTestEntry.Attributes.Append(testId);
            xTestEntry.Attributes.Append(executionId);
            xTestEntry.Attributes.Append(testListId);

            return xTestEntry;
        }

        private static XmlElement GetUnitTest(TestOutcome outcome)
        {

            var testMethodInfo = typeof(TestSuiteMethodBase)
                        .GetField("TestMethod", BindingFlags.NonPublic | BindingFlags.Instance)
                        .GetValue(outcome) as MethodInfo;

            var unitTest = trx.CreateElement(string.Empty, "UnitTest", string.Empty);

            var name = trx.CreateAttribute("name");
            name.Value = outcome.FullTestName;

            var storage = trx.CreateAttribute("storage");
            storage.Value = Path.GetFileName(testMethodInfo.DeclaringType.Assembly.Location);

            var id = trx.CreateAttribute("id");
            id.Value = outcome.Id.ToString();

            unitTest.Attributes.Append(name);
            unitTest.Attributes.Append(storage);
            unitTest.Attributes.Append(id);

            var description = trx.CreateElement(string.Empty, "Description", string.Empty);
            description.InnerText = outcome.Description;

            var execution = trx.CreateElement(string.Empty, "Execution", string.Empty);

            var executionId = trx.CreateAttribute("id");
            executionId.Value = GetGuid(outcome.Outcome.StartTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK"));

            execution.Attributes.Append(executionId);

            var testMethod = trx.CreateElement(string.Empty, "TestMethod", string.Empty);

            var codeBase = trx.CreateAttribute("codeBase");
            codeBase.Value = testMethodInfo.DeclaringType.Assembly.CodeBase.Replace("file:///", string.Empty);

            var adapterTypeName = trx.CreateAttribute("adapterTypeName");
            adapterTypeName.Value = Adapter;

            var className = trx.CreateAttribute("className");
            className.Value = testMethodInfo.DeclaringType.AssemblyQualifiedName;

            var aName = trx.CreateAttribute("name");
            aName.Value = outcome.FullTestName;

            testMethod.Attributes.Append(codeBase);
            testMethod.Attributes.Append(adapterTypeName);
            testMethod.Attributes.Append(className);
            testMethod.Attributes.Append(aName);

            unitTest.AppendChild(description);
            unitTest.AppendChild(execution);
            unitTest.AppendChild(testMethod);

            return unitTest;
        }

        private static string GetGuid(string value)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.Default.GetBytes(value));
                return new Guid(hash).ToString();
            }
        }
    }
}
