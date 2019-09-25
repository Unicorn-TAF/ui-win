using System;
using System.Linq;
using System.Text;
using System.Xml;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Testing;

namespace Unicorn.Taf.Core.Utility
{
    /// <summary>
    /// Provides with ability to generate TRX file based on <see cref="LaunchOutcome"/>
    /// </summary>
    public class TrxCreator
    {
        private const string Adapter = "Microsoft.VisualStudio.TestTools.TestTypes.Unit.UnitTestAdapter, Microsoft.VisualStudio.QualityTools.Tips.UnitTest.Adapter, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
        private const string DtFormat = "yyyy-MM-ddTHH:mm:ss.fffffffK";

        private readonly XmlDocument trx;
        private readonly XmlElement testDefinitions;
        private readonly XmlElement testLists;
        private readonly XmlElement testEntries;
        private readonly XmlElement results;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="TrxCreator"/> class.
        /// </summary>
        public TrxCreator()
        {
            this.trx = new XmlDocument();
            var xmlDeclaration = trx.CreateXmlDeclaration("1.0", "UTF-8", null);
            var root = trx.DocumentElement;
            trx.InsertBefore(xmlDeclaration, root);

            testDefinitions = trx.CreateElement(string.Empty, "TestDefinitions", string.Empty);
            testLists = trx.CreateElement(string.Empty, "TestLists", string.Empty);
            testEntries = trx.CreateElement(string.Empty, "TestEntries", string.Empty);
            results = trx.CreateElement(string.Empty, "Results", string.Empty);
        }

        /// <summary>
        /// Generates TRX file for <see cref="LaunchOutcome"/> and saves it by specified path
        /// </summary>
        /// <param name="outcome">tests run outcome</param>
        /// <param name="trxPath">resulting TRX full file name</param>
        public void GenerateTrxFile(LaunchOutcome outcome, string trxPath)
        {
            if (!outcome.SuitesOutcomes.Any())
            {
                return;
            }

            try
            {
                var testRun = GetTestRun(outcome.StartTime);

                foreach (var suiteOutcome in outcome.SuitesOutcomes)
                {
                    FillTrxWithSuiteData(suiteOutcome);
                }

                var resultSummary = GetResultSummary();

                testRun.AppendChild(resultSummary);
                testRun.AppendChild(testDefinitions);
                testRun.AppendChild(testLists);
                testRun.AppendChild(testEntries);
                testRun.AppendChild(results);

                trx.AppendChild(testRun);

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
            var testRun = trx.CreateElement(string.Empty, "TestRun", string.Empty);

            var xmlns = trx.CreateAttribute("xmlns");
            xmlns.Value = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010";

            var runId = trx.CreateAttribute("id");
            runId.Value = Guid.NewGuid().ToString();

            var runName = trx.CreateAttribute("name");
            runName.Value = "Tests run: " + DateTime.Now;

            var runUser = trx.CreateAttribute("runUser");
            runUser.Value = Environment.UserName;

            // Generate "Times" section.
            var times = trx.CreateElement(string.Empty, "Times", string.Empty);

            var creation = trx.CreateAttribute("creation");
            creation.Value = startTime.AddMilliseconds(-20).ToString(DtFormat);

            var queuing = trx.CreateAttribute("queuing");
            queuing.Value = startTime.AddMilliseconds(-10).ToString(DtFormat);

            var start = trx.CreateAttribute("start");
            start.Value = startTime.ToString(DtFormat);

            var finish = trx.CreateAttribute("finish");
            finish.Value = DateTime.Now.ToString(DtFormat);

            times.Attributes.Append(creation);
            times.Attributes.Append(queuing);
            times.Attributes.Append(start);
            times.Attributes.Append(finish);

            testRun.Attributes.Append(runId);
            testRun.Attributes.Append(runName);
            testRun.Attributes.Append(runUser);
            testRun.Attributes.Append(xmlns);
            testRun.AppendChild(times);

            return testRun;
        }

        private XmlElement GetUnitTest(TestOutcome outcome, string executionIdValue, Guid idValue)
        {
            var unitTest = trx.CreateElement(string.Empty, "UnitTest", string.Empty);

            var name = trx.CreateAttribute("name");
            name.Value = outcome.FullMethodName;

            var storage = trx.CreateAttribute("storage");
            storage.Value = string.Empty; ////TODO: Path.GetFileName(outcome.testMethodInfo.DeclaringType.Assembly.Location));

            var id = trx.CreateAttribute("id");

            id.Value = idValue.ToString();

            unitTest.Attributes.Append(name);
            unitTest.Attributes.Append(storage);
            unitTest.Attributes.Append(id);

            // Description.
            var description = trx.CreateElement(string.Empty, "Description", string.Empty);
            description.InnerText = outcome.Title;

            // Execution.
            var execution = trx.CreateElement(string.Empty, "Execution", string.Empty);

            var executionId = trx.CreateAttribute("id");
            executionId.Value = executionIdValue;

            execution.Attributes.Append(executionId);

            // TestMethod.
            var testMethod = trx.CreateElement(string.Empty, "TestMethod", string.Empty);

            var codeBase = trx.CreateAttribute("codeBase");
            codeBase.Value = string.Empty; ////TODO: testMethodInfo.DeclaringType.Assembly.CodeBase.Replace("file:///", string.Empty);

            var adapterTypeName = trx.CreateAttribute("adapterTypeName");
            adapterTypeName.Value = Adapter;

            var className = trx.CreateAttribute("className");
            className.Value = string.Empty; ////TODO: testMethodInfo.DeclaringType.AssemblyQualifiedName;

            var testMethodName = trx.CreateAttribute("name");
            testMethodName.Value = outcome.FullMethodName;

            testMethod.Attributes.Append(codeBase);
            testMethod.Attributes.Append(adapterTypeName);
            testMethod.Attributes.Append(className);
            testMethod.Attributes.Append(testMethodName);

            unitTest.AppendChild(description);
            unitTest.AppendChild(execution);
            unitTest.AppendChild(testMethod);

            return unitTest;
        }

        private void FillTrxWithSuiteData(SuiteOutcome outcome)
        {
            var testList = GetTestList(outcome);
            testLists.AppendChild(testList);

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

                var testId = string.IsNullOrEmpty(outcome.DataSetName) ?
                    testOutcome.Id :
                    AdapterUtilities.GuidFromString(testOutcome.FullMethodName + $"[{outcome.DataSetName}]");

                var unitTest = GetUnitTest(testOutcome, executionIdValue, testId);
                var testEntry = GetTestEntry(testOutcome, executionIdValue, testId);
                var unitTestResult = GetUnitTestResult(testOutcome, executionIdValue, testId);

                testDefinitions.AppendChild(unitTest);
                testEntries.AppendChild(testEntry);
                results.AppendChild(unitTestResult);
            }
        }

        private XmlElement GetTestList(SuiteOutcome outcome)
        {
            var testList = trx.CreateElement(string.Empty, "TestList", string.Empty);

            var name = trx.CreateAttribute("name");

            var nameValue = outcome.Name;

            if (!string.IsNullOrEmpty(outcome.DataSetName))
            {
                nameValue += $"[{outcome.DataSetName}]";
            }

            name.Value = nameValue;

            var id = trx.CreateAttribute("id");
            id.Value = outcome.Id.ToString();

            testList.Attributes.Append(name);
            testList.Attributes.Append(id);

            return testList;
        }

        private XmlElement GetUnitTestResult(TestOutcome outcome, string executionIdValue, Guid idValue)
        {
            var unitTestResult = trx.CreateElement(string.Empty, "UnitTestResult", string.Empty);

            var executionId = trx.CreateAttribute("executionId");
            executionId.Value = executionIdValue;

            var testId = trx.CreateAttribute("testId");

            testId.Value = idValue.ToString();

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

            var outcomeAttribute = trx.CreateAttribute("outcome");

            switch (outcome.Result)
            {
                case Status.Passed:
                    outcomeAttribute.Value = "Passed";
                    break;
                case Status.Failed:
                    outcomeAttribute.Value = "Failed";
                    break;
                case Status.Skipped:
                    outcomeAttribute.Value = "Inconclusive";
                    break;
            }

            var testListId = trx.CreateAttribute("testListId");
            testListId.Value = outcome.ParentId.ToString();

            unitTestResult.Attributes.Append(executionId);
            unitTestResult.Attributes.Append(testId);
            unitTestResult.Attributes.Append(testName);
            unitTestResult.Attributes.Append(computerName);
            unitTestResult.Attributes.Append(duration);
            unitTestResult.Attributes.Append(startTime);
            unitTestResult.Attributes.Append(endTime);
            unitTestResult.Attributes.Append(testType);
            unitTestResult.Attributes.Append(outcomeAttribute);
            unitTestResult.Attributes.Append(testListId);

            var output = trx.CreateElement(string.Empty, "Output", string.Empty);
            var stdOut = trx.CreateElement(string.Empty, "StdOut", string.Empty);

            // Generate "FailInfo" section.
            if (outcome.Result.Equals(Status.Failed))
            {
                var errorInfo = trx.CreateElement(string.Empty, "ErrorInfo", string.Empty);
                var message = trx.CreateElement(string.Empty, "Message", string.Empty);
                var stackTrace = trx.CreateElement(string.Empty, "StackTrace", string.Empty);

                message.InnerText = outcome.Exception.Message;
                stackTrace.InnerText = outcome.Exception.StackTrace;
                stdOut.InnerText = outcome.Output;

                errorInfo.AppendChild(message);
                errorInfo.AppendChild(stackTrace);
                output.AppendChild(errorInfo);
            }

            output.AppendChild(stdOut);

            unitTestResult.AppendChild(output);

            return unitTestResult;
        }

        private XmlElement GetTestEntry(TestOutcome outcome, string executionIdValue, Guid idValue)
        {
            var testEntry = trx.CreateElement(string.Empty, "TestEntry", string.Empty);

            var testId = trx.CreateAttribute("testId");

            testId.Value = idValue.ToString();

            var executionId = trx.CreateAttribute("executionId");
            executionId.Value = executionIdValue;

            var testListId = trx.CreateAttribute("testListId");
            testListId.Value = outcome.ParentId.ToString();

            testEntry.Attributes.Append(testId);
            testEntry.Attributes.Append(executionId);
            testEntry.Attributes.Append(testListId);

            return testEntry;
        }

        private XmlElement GetResultSummary()
        {
            var resultSummary = trx.CreateElement(string.Empty, "ResultSummary", string.Empty);
            var counters = trx.CreateElement(string.Empty, "Counters", string.Empty);

            var totalAttribute = trx.CreateAttribute("total");
            totalAttribute.Value = total.ToString();

            var executedAttribute = trx.CreateAttribute("executed");
            executedAttribute.Value = executed.ToString();

            var passedAttribute = trx.CreateAttribute("passed");
            passedAttribute.Value = passed.ToString();

            var errorAttribute = trx.CreateAttribute("error");
            errorAttribute.Value = error.ToString();

            var failedAttribute = trx.CreateAttribute("failed");
            failedAttribute.Value = failed.ToString();

            var timeoutAttribute = trx.CreateAttribute("timeout");
            timeoutAttribute.Value = timeout.ToString();

            var abortedAttribute = trx.CreateAttribute("aborted");
            abortedAttribute.Value = aborted.ToString();

            var inconclusiveAttribute = trx.CreateAttribute("inconclusive");
            inconclusiveAttribute.Value = inconclusive.ToString();

            var passedButRunAbortedAttribute = trx.CreateAttribute("passedButRunAborted");
            passedButRunAbortedAttribute.Value = passedButRunAborted.ToString();

            var notRunnableAttribute = trx.CreateAttribute("notRunnable");
            notRunnableAttribute.Value = notRunnable.ToString();

            var notExecutedAttribute = trx.CreateAttribute("notExecuted");
            notExecutedAttribute.Value = notExecuted.ToString();

            var disconnectedAttribute = trx.CreateAttribute("disconnected");
            disconnectedAttribute.Value = disconnected.ToString();

            var warningAttribute = trx.CreateAttribute("warning");
            warningAttribute.Value = warning.ToString();

            var completedAttribute = trx.CreateAttribute("completed");
            completedAttribute.Value = completed.ToString();

            var inProgressAttribute = trx.CreateAttribute("inProgress");
            inProgressAttribute.Value = inProgress.ToString();

            var pendingAttribute = trx.CreateAttribute("pending");
            pendingAttribute.Value = pending.ToString();

            counters.Attributes.Append(totalAttribute);
            counters.Attributes.Append(executedAttribute);
            counters.Attributes.Append(passedAttribute);
            counters.Attributes.Append(errorAttribute);
            counters.Attributes.Append(failedAttribute);
            counters.Attributes.Append(timeoutAttribute);
            counters.Attributes.Append(abortedAttribute);
            counters.Attributes.Append(inconclusiveAttribute);
            counters.Attributes.Append(passedButRunAbortedAttribute);
            counters.Attributes.Append(notRunnableAttribute);
            counters.Attributes.Append(notExecutedAttribute);
            counters.Attributes.Append(disconnectedAttribute);
            counters.Attributes.Append(warningAttribute);
            counters.Attributes.Append(completedAttribute);
            counters.Attributes.Append(inProgressAttribute);
            counters.Attributes.Append(pendingAttribute);

            resultSummary.AppendChild(counters);

            return resultSummary;
        }
    }
}
