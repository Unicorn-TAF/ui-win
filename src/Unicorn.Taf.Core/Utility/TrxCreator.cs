using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Internal;
using Unicorn.Taf.Core.Testing;

namespace Unicorn.Taf.Core.Utility
{
    /// <summary>
    /// Provides with ability to generate TRX file based on <see cref="LaunchOutcome"/>
    /// </summary>
    public class TrxCreator
    {
        private const string DtFormat = "yyyy-MM-ddTHH:mm:ss.fffffffK";

        private const string Adapter = "executor://UnicornTestExecutor/v2";

        private readonly XmlDocument _trx;
        private readonly XmlElement _testDefinitions;
        private readonly XmlElement _testLists;
        private readonly XmlElement _testEntries;
        private readonly XmlElement _results;
        private readonly string _machineName = Environment.MachineName;
        private readonly string _assemblyLocation = string.Empty;

        private readonly int _error = 0;
        private readonly int _timeout = 0;
        private readonly int _aborted = 0;
        private readonly int _passedButRunAborted = 0;
        private readonly int _notRunnable = 0;
        private readonly int _disconnected = 0;
        private readonly int _warning = 0;
        private readonly int _inProgress = 0;
        private readonly int _pending = 0;

        private readonly string[] _xmlInvalidSymbols = { "&#x0" };

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
        /// Initializes a new instance of the <see cref="TrxCreator"/> class based on test assembly 
        /// (codebase and storage are populated based on assembly info).
        /// </summary>
        public TrxCreator(Assembly testAssembly) : this()
        {
            _assemblyLocation = testAssembly.Location;
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

                var resultSummary = GetResultSummary(outcome);

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

        private XmlElement GetUnitTest(TestOutcome outcome, string executionIdValue)
        {
            var unitTest = _trx.CreateElement(string.Empty, "UnitTest", string.Empty);

            var name = _trx.CreateAttribute("name");
            name.Value = GetMethodName(outcome.FullMethodName);
            unitTest.Attributes.Append(name);

            var storage = _trx.CreateAttribute("storage");
            storage.Value = _assemblyLocation;
            unitTest.Attributes.Append(storage);

            var id = _trx.CreateAttribute("id");
            id.Value = outcome.Id.ToString();
            unitTest.Attributes.Append(id);

            // Description.
            var description = _trx.CreateElement(string.Empty, "Description", string.Empty);
            description.InnerText = outcome.Title;
            unitTest.AppendChild(description);

            // Execution.
            var execution = _trx.CreateElement(string.Empty, "Execution", string.Empty);

            var executionId = _trx.CreateAttribute("id");
            executionId.Value = executionIdValue;
            execution.Attributes.Append(executionId);
            
            unitTest.AppendChild(execution);

            // TestMethod.
            var testMethod = _trx.CreateElement(string.Empty, "TestMethod", string.Empty);

            var codeBase = _trx.CreateAttribute("codeBase");
            codeBase.Value = _assemblyLocation;
            testMethod.Attributes.Append(codeBase);

            var adapterTypeName = _trx.CreateAttribute("adapterTypeName");
            adapterTypeName.Value = Adapter;
            testMethod.Attributes.Append(adapterTypeName);

            var className = _trx.CreateAttribute("className");
            className.Value = outcome.FullMethodName
                .Substring(0, outcome.FullMethodName.LastIndexOf("."));
            testMethod.Attributes.Append(className);

            var testMethodName = _trx.CreateAttribute("name");
            testMethodName.Value = GetMethodName(outcome.FullMethodName);
            testMethod.Attributes.Append(testMethodName);

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

                var executionIdValue = GuidGenerator.FromString(testOutcome.StartTime.ToString() + testOutcome.Id).ToString();

                var unitTest = GetUnitTest(testOutcome, executionIdValue);
                var testEntry = GetTestEntry(testOutcome, executionIdValue);
                var unitTestResult = GetUnitTestResult(testOutcome, executionIdValue);

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
            testList.Attributes.Append(name);

            var id = _trx.CreateAttribute("id");
            id.Value = outcome.Id.ToString();
            testList.Attributes.Append(id);

            return testList;
        }

        private XmlElement GetUnitTestResult(TestOutcome outcome, string executionIdValue)
        {
            var unitTestResult = _trx.CreateElement(string.Empty, "UnitTestResult", string.Empty);

            var executionId = _trx.CreateAttribute("executionId");
            executionId.Value = executionIdValue;
            unitTestResult.Attributes.Append(executionId);

            var testId = _trx.CreateAttribute("testId");
            testId.Value = outcome.Id.ToString();
            unitTestResult.Attributes.Append(testId);

            var testName = _trx.CreateAttribute("testName");
            testName.Value = GetMethodName(outcome.FullMethodName);
            unitTestResult.Attributes.Append(testName);

            var computerName = _trx.CreateAttribute("computerName");
            computerName.Value = _machineName;
            unitTestResult.Attributes.Append(computerName);

            var duration = _trx.CreateAttribute("duration");
            duration.Value = string.Format("{0:hh\\:mm\\:ss\\.fffffff}", outcome.ExecutionTime);
            unitTestResult.Attributes.Append(duration);

            var startTime = _trx.CreateAttribute("startTime");
            startTime.Value = outcome.StartTime.ToString(DtFormat, CultureInfo.InvariantCulture);
            unitTestResult.Attributes.Append(startTime);

            var endTime = _trx.CreateAttribute("endTime");
            endTime.Value = outcome.StartTime.Add(outcome.ExecutionTime).ToString(DtFormat, CultureInfo.InvariantCulture);
            unitTestResult.Attributes.Append(endTime);

            var testType = _trx.CreateAttribute("testType");
            testType.Value = "13cdc9d9-ddb5-4fa4-a97d-d965ccfc6d4b";
            unitTestResult.Attributes.Append(testType);

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

            unitTestResult.Attributes.Append(outcomeAttribute);


            var testListId = _trx.CreateAttribute("testListId");
            testListId.Value = outcome.ParentId.ToString();
            unitTestResult.Attributes.Append(testListId);

            var output = _trx.CreateElement(string.Empty, "Output", string.Empty);
            var stdOut = _trx.CreateElement(string.Empty, "StdOut", string.Empty);

            // Generate "FailInfo" section.
            if (outcome.Result.Equals(Status.Failed))
            {
                var errorInfo = _trx.CreateElement(string.Empty, "ErrorInfo", string.Empty);
                
                var message = _trx.CreateElement(string.Empty, "Message", string.Empty);
                message.InnerText = outcome.Exception.Message;
                errorInfo.AppendChild(message);

                var stackTrace = _trx.CreateElement(string.Empty, "StackTrace", string.Empty);
                stackTrace.InnerText = outcome.Exception.StackTrace;
                errorInfo.AppendChild(stackTrace);
                
                output.AppendChild(errorInfo);

                stdOut.InnerText = RemoveXmlInvalidSymbolsFromString(outcome.Output);
            }

            output.AppendChild(stdOut);

            unitTestResult.AppendChild(output);

            return unitTestResult;
        }

        private XmlElement GetTestEntry(TestOutcome outcome, string executionIdValue)
        {
            var testEntry = _trx.CreateElement(string.Empty, "TestEntry", string.Empty);

            var testId = _trx.CreateAttribute("testId");
            testId.Value = outcome.Id.ToString();
            testEntry.Attributes.Append(testId);

            var executionId = _trx.CreateAttribute("executionId");
            executionId.Value = executionIdValue;
            testEntry.Attributes.Append(executionId);

            var testListId = _trx.CreateAttribute("testListId");
            testListId.Value = outcome.ParentId.ToString();
            testEntry.Attributes.Append(testListId);

            return testEntry;
        }

        private XmlElement GetResultSummary(LaunchOutcome outcome)
        {
            var resultSummary = _trx.CreateElement(string.Empty, "ResultSummary", string.Empty);

            var outcomeAttribute = _trx.CreateAttribute("outcome");
            outcomeAttribute.Value = outcome.RunStatus.ToString();
            resultSummary.Attributes.Append(outcomeAttribute);

            var counters = _trx.CreateElement(string.Empty, "Counters", string.Empty);

            var totalAttribute = _trx.CreateAttribute("total");
            totalAttribute.Value = total.ToString();
            counters.Attributes.Append(totalAttribute);

            var executedAttribute = _trx.CreateAttribute("executed");
            executedAttribute.Value = executed.ToString();
            counters.Attributes.Append(executedAttribute);

            var passedAttribute = _trx.CreateAttribute("passed");
            passedAttribute.Value = passed.ToString();
            counters.Attributes.Append(passedAttribute);

            var errorAttribute = _trx.CreateAttribute("error");
            errorAttribute.Value = _error.ToString();
            counters.Attributes.Append(errorAttribute);

            var failedAttribute = _trx.CreateAttribute("failed");
            failedAttribute.Value = failed.ToString();
            counters.Attributes.Append(failedAttribute);

            var timeoutAttribute = _trx.CreateAttribute("timeout");
            timeoutAttribute.Value = _timeout.ToString();
            counters.Attributes.Append(timeoutAttribute);

            var abortedAttribute = _trx.CreateAttribute("aborted");
            abortedAttribute.Value = _aborted.ToString();
            counters.Attributes.Append(abortedAttribute);

            var inconclusiveAttribute = _trx.CreateAttribute("inconclusive");
            inconclusiveAttribute.Value = inconclusive.ToString();
            counters.Attributes.Append(inconclusiveAttribute);

            var passedButRunAbortedAttribute = _trx.CreateAttribute("passedButRunAborted");
            passedButRunAbortedAttribute.Value = _passedButRunAborted.ToString();
            counters.Attributes.Append(passedButRunAbortedAttribute);

            var notRunnableAttribute = _trx.CreateAttribute("notRunnable");
            notRunnableAttribute.Value = _notRunnable.ToString();
            counters.Attributes.Append(notRunnableAttribute);

            var notExecutedAttribute = _trx.CreateAttribute("notExecuted");
            notExecutedAttribute.Value = notExecuted.ToString();
            counters.Attributes.Append(notExecutedAttribute);

            var disconnectedAttribute = _trx.CreateAttribute("disconnected");
            disconnectedAttribute.Value = _disconnected.ToString();
            counters.Attributes.Append(disconnectedAttribute);

            var warningAttribute = _trx.CreateAttribute("warning");
            warningAttribute.Value = _warning.ToString();
            counters.Attributes.Append(warningAttribute);

            var completedAttribute = _trx.CreateAttribute("completed");
            completedAttribute.Value = completed.ToString();
            counters.Attributes.Append(completedAttribute);

            var inProgressAttribute = _trx.CreateAttribute("inProgress");
            inProgressAttribute.Value = _inProgress.ToString();
            counters.Attributes.Append(inProgressAttribute);

            var pendingAttribute = _trx.CreateAttribute("pending");
            pendingAttribute.Value = _pending.ToString();
            counters.Attributes.Append(pendingAttribute);

            resultSummary.AppendChild(counters);

            return resultSummary;
        }

        private string RemoveXmlInvalidSymbolsFromString(string input)
        {
            StringBuilder sb = new StringBuilder(input);

            foreach (var invalidSymbol in _xmlInvalidSymbols)
            {
                sb.Replace(invalidSymbol, string.Empty);
            }

            return sb.ToString().ToLower();
        }

        private string GetMethodName(string fullMethodNme) =>
            fullMethodNme.Split('.').Last();
    }
}