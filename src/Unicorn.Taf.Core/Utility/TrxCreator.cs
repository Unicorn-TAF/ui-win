using System;
using System.Globalization;
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

        private readonly XmlDocument _trx;
        private readonly XmlElement _testDefinitions;
        private readonly XmlElement _testLists;
        private readonly XmlElement _testEntries;
        private readonly XmlElement _results;
        private readonly string _machineName = Environment.MachineName;

        private readonly int _error = 0;
        private readonly int _timeout = 0;
        private readonly int _aborted = 0;
        private readonly int _passedButRunAborted = 0;
        private readonly int _notRunnable = 0;
        private readonly int _disconnected = 0;
        private readonly int _warning = 0;
        private readonly int _inProgress = 0;
        private readonly int _pending = 0;

        private readonly string[] xmlInvalidSymbols = { "&#x0" };

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
            _trx = new XmlDocument();
            var xmlDeclaration = _trx.CreateXmlDeclaration("1.0", "UTF-8", null);
            var root = _trx.DocumentElement;
            _trx.InsertBefore(xmlDeclaration, root);

            _testDefinitions = _trx.CreateElement(string.Empty, "TestDefinitions", string.Empty);
            _testLists = _trx.CreateElement(string.Empty, "TestLists", string.Empty);
            _testEntries = _trx.CreateElement(string.Empty, "TestEntries", string.Empty);
            _results = _trx.CreateElement(string.Empty, "Results", string.Empty);
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
                testRun.AppendChild(_testDefinitions);
                testRun.AppendChild(_testLists);
                testRun.AppendChild(_testEntries);
                testRun.AppendChild(_results);

                _trx.AppendChild(testRun);

                using (var writer = new XmlTextWriter(trxPath, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;
                    _trx.Save(writer);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("TRX test results file generation failed. " + e);
            }
        }

        private XmlElement GetTestRun(DateTime startTime)
        {
            var testRun = _trx.CreateElement(string.Empty, "TestRun", string.Empty);

            var xmlns = _trx.CreateAttribute("xmlns");
            xmlns.Value = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010";

            var runId = _trx.CreateAttribute("id");
            runId.Value = Guid.NewGuid().ToString();

            var runName = _trx.CreateAttribute("name");
            runName.Value = "Tests run: " + DateTime.Now;

            var runUser = _trx.CreateAttribute("runUser");
            runUser.Value = Environment.UserName;

            // Generate "Times" section.
            var times = _trx.CreateElement(string.Empty, "Times", string.Empty);

            var creation = _trx.CreateAttribute("creation");
            creation.Value = startTime.AddMilliseconds(-20).ToString(DtFormat, CultureInfo.InvariantCulture);

            var queuing = _trx.CreateAttribute("queuing");
            queuing.Value = startTime.AddMilliseconds(-10).ToString(DtFormat, CultureInfo.InvariantCulture);

            var start = _trx.CreateAttribute("start");
            start.Value = startTime.ToString(DtFormat, CultureInfo.InvariantCulture);

            var finish = _trx.CreateAttribute("finish");
            finish.Value = DateTime.Now.ToString(DtFormat, CultureInfo.InvariantCulture);

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
            var unitTest = _trx.CreateElement(string.Empty, "UnitTest", string.Empty);

            var name = _trx.CreateAttribute("name");
            name.Value = outcome.FullMethodName;

            var storage = _trx.CreateAttribute("storage");
            storage.Value = string.Empty; ////TODO: Path.GetFileName(outcome.testMethodInfo.DeclaringType.Assembly.Location));

            var id = _trx.CreateAttribute("id");

            id.Value = idValue.ToString();

            unitTest.Attributes.Append(name);
            unitTest.Attributes.Append(storage);
            unitTest.Attributes.Append(id);

            // Description.
            var description = _trx.CreateElement(string.Empty, "Description", string.Empty);
            description.InnerText = outcome.Title;

            // Execution.
            var execution = _trx.CreateElement(string.Empty, "Execution", string.Empty);

            var executionId = _trx.CreateAttribute("id");
            executionId.Value = executionIdValue;

            execution.Attributes.Append(executionId);

            // TestMethod.
            var testMethod = _trx.CreateElement(string.Empty, "TestMethod", string.Empty);

            var codeBase = _trx.CreateAttribute("codeBase");
            codeBase.Value = string.Empty; ////TODO: testMethodInfo.DeclaringType.Assembly.CodeBase.Replace("file:///", string.Empty);

            var adapterTypeName = _trx.CreateAttribute("adapterTypeName");
            adapterTypeName.Value = Adapter;

            var className = _trx.CreateAttribute("className");
            className.Value = string.Empty; ////TODO: testMethodInfo.DeclaringType.AssemblyQualifiedName;

            var testMethodName = _trx.CreateAttribute("name");
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
            _testLists.AppendChild(testList);

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

                var executionIdValue = AdapterUtilities.GuidFromString(testOutcome.StartTime.ToString(DtFormat, CultureInfo.InvariantCulture)).ToString();

                var testId = string.IsNullOrEmpty(outcome.DataSetName) ?
                    testOutcome.Id :
                    AdapterUtilities.GuidFromString(testOutcome.FullMethodName + $"[{outcome.DataSetName}]");

                var unitTest = GetUnitTest(testOutcome, executionIdValue, testId);
                var testEntry = GetTestEntry(testOutcome, executionIdValue, testId);
                var unitTestResult = GetUnitTestResult(testOutcome, executionIdValue, testId);

                _testDefinitions.AppendChild(unitTest);
                _testEntries.AppendChild(testEntry);
                _results.AppendChild(unitTestResult);
            }
        }

        private XmlElement GetTestList(SuiteOutcome outcome)
        {
            var testList = _trx.CreateElement(string.Empty, "TestList", string.Empty);

            var name = _trx.CreateAttribute("name");

            var nameValue = outcome.Name;

            if (!string.IsNullOrEmpty(outcome.DataSetName))
            {
                nameValue += $"[{outcome.DataSetName}]";
            }

            name.Value = nameValue;

            var id = _trx.CreateAttribute("id");
            id.Value = outcome.Id.ToString();

            testList.Attributes.Append(name);
            testList.Attributes.Append(id);

            return testList;
        }

        private XmlElement GetUnitTestResult(TestOutcome outcome, string executionIdValue, Guid idValue)
        {
            var unitTestResult = _trx.CreateElement(string.Empty, "UnitTestResult", string.Empty);

            var executionId = _trx.CreateAttribute("executionId");
            executionId.Value = executionIdValue;

            var testId = _trx.CreateAttribute("testId");

            testId.Value = idValue.ToString();

            var testName = _trx.CreateAttribute("testName");
            testName.Value = outcome.FullMethodName;

            var computerName = _trx.CreateAttribute("computerName");
            computerName.Value = _machineName;

            var duration = _trx.CreateAttribute("duration");
            duration.Value = string.Format("{0:hh\\:mm\\:ss\\.fffffff}", outcome.ExecutionTime);

            var startTime = _trx.CreateAttribute("startTime");
            startTime.Value = outcome.StartTime.ToString(DtFormat, CultureInfo.InvariantCulture);

            var endTime = _trx.CreateAttribute("endTime");
            endTime.Value = outcome.StartTime.Add(outcome.ExecutionTime).ToString(DtFormat, CultureInfo.InvariantCulture);

            var testType = _trx.CreateAttribute("testType");
            testType.Value = "13cdc9d9-ddb5-4fa4-a97d-d965ccfc6d4b";

            var outcomeAttribute = _trx.CreateAttribute("outcome");

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

            var testListId = _trx.CreateAttribute("testListId");
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

            var output = _trx.CreateElement(string.Empty, "Output", string.Empty);
            var stdOut = _trx.CreateElement(string.Empty, "StdOut", string.Empty);

            // Generate "FailInfo" section.
            if (outcome.Result.Equals(Status.Failed))
            {
                var errorInfo = _trx.CreateElement(string.Empty, "ErrorInfo", string.Empty);
                var message = _trx.CreateElement(string.Empty, "Message", string.Empty);
                var stackTrace = _trx.CreateElement(string.Empty, "StackTrace", string.Empty);

                message.InnerText = outcome.Exception.Message;
                stackTrace.InnerText = outcome.Exception.StackTrace;
                stdOut.InnerText = RemoveXmlInvalidSymbolsFromString(outcome.Output);

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
            var testEntry = _trx.CreateElement(string.Empty, "TestEntry", string.Empty);

            var testId = _trx.CreateAttribute("testId");

            testId.Value = idValue.ToString();

            var executionId = _trx.CreateAttribute("executionId");
            executionId.Value = executionIdValue;

            var testListId = _trx.CreateAttribute("testListId");
            testListId.Value = outcome.ParentId.ToString();

            testEntry.Attributes.Append(testId);
            testEntry.Attributes.Append(executionId);
            testEntry.Attributes.Append(testListId);

            return testEntry;
        }

        private XmlElement GetResultSummary()
        {
            var resultSummary = _trx.CreateElement(string.Empty, "ResultSummary", string.Empty);
            var counters = _trx.CreateElement(string.Empty, "Counters", string.Empty);

            var totalAttribute = _trx.CreateAttribute("total");
            totalAttribute.Value = total.ToString();

            var executedAttribute = _trx.CreateAttribute("executed");
            executedAttribute.Value = executed.ToString();

            var passedAttribute = _trx.CreateAttribute("passed");
            passedAttribute.Value = passed.ToString();

            var errorAttribute = _trx.CreateAttribute("error");
            errorAttribute.Value = _error.ToString();

            var failedAttribute = _trx.CreateAttribute("failed");
            failedAttribute.Value = failed.ToString();

            var timeoutAttribute = _trx.CreateAttribute("timeout");
            timeoutAttribute.Value = _timeout.ToString();

            var abortedAttribute = _trx.CreateAttribute("aborted");
            abortedAttribute.Value = _aborted.ToString();

            var inconclusiveAttribute = _trx.CreateAttribute("inconclusive");
            inconclusiveAttribute.Value = inconclusive.ToString();

            var passedButRunAbortedAttribute = _trx.CreateAttribute("passedButRunAborted");
            passedButRunAbortedAttribute.Value = _passedButRunAborted.ToString();

            var notRunnableAttribute = _trx.CreateAttribute("notRunnable");
            notRunnableAttribute.Value = _notRunnable.ToString();

            var notExecutedAttribute = _trx.CreateAttribute("notExecuted");
            notExecutedAttribute.Value = notExecuted.ToString();

            var disconnectedAttribute = _trx.CreateAttribute("disconnected");
            disconnectedAttribute.Value = _disconnected.ToString();

            var warningAttribute = _trx.CreateAttribute("warning");
            warningAttribute.Value = _warning.ToString();

            var completedAttribute = _trx.CreateAttribute("completed");
            completedAttribute.Value = completed.ToString();

            var inProgressAttribute = _trx.CreateAttribute("inProgress");
            inProgressAttribute.Value = _inProgress.ToString();

            var pendingAttribute = _trx.CreateAttribute("pending");
            pendingAttribute.Value = _pending.ToString();

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

        private string RemoveXmlInvalidSymbolsFromString(string input)
        {
            StringBuilder sb = new StringBuilder(input);

            foreach (var invalidSymbol in xmlInvalidSymbols)
            {
                sb.Replace(invalidSymbol, string.Empty);
            }

            return sb.ToString().ToLower();
        }
    }
}