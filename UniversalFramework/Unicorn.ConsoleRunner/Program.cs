using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Unicorn.Core.Testing.Tests;
using Unicorn.Core.Testing.Tests.Adapter;

namespace Unicorn.ConsoleRunner
{
    class Program
    {
        private const string ConstTestAssembly = "testAssembly";
        private const string ConstConfiguration = "configuration";
        private static readonly string delimiter = new string('-', 123);

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                PrintHelpText();
                throw new ArgumentException("Required parameters were not specified");
            }

            string assemblyPath = null;
            string propertiesPath = null;

            var assemblyArgs = args.Where(a => a.Trim().StartsWith(ConstTestAssembly));

            if(assemblyArgs.Any())
            {
                assemblyPath = assemblyArgs.First().Trim().Split('=')[1].Trim();
            }
            else
            {
                PrintHelpText();
                throw new ArgumentException($"'{ConstTestAssembly}' parameter was not specified");
            }


            var configArgs = args.Where(a => a.Trim().StartsWith(ConstConfiguration));

            if (configArgs.Any())
            {
                propertiesPath = configArgs.First().Trim().Split('=')[1].Trim();
            }
            else
            {
                PrintHelpText();
                throw new ArgumentException($"'{ConstConfiguration}' parameter was not specified");
            }

            Uri assemblyUri;

            if (Path.IsPathRooted(assemblyPath))
            {
                assemblyUri = new Uri(assemblyPath, UriKind.Absolute);
            }
            else
            {
                assemblyUri = new Uri(assemblyPath, UriKind.Relative);
            }

            Uri configUri;

            if (Path.IsPathRooted(propertiesPath))
            {
                configUri = new Uri(propertiesPath, UriKind.Absolute);
            }
            else
            {
                configUri = new Uri(propertiesPath, UriKind.Relative);
            }

            TestsRunner runner = new TestsRunner(Assembly.LoadFrom(assemblyUri.ToString()), configUri.ToString());

            ReportHeader(assemblyPath);

            runner.RunTests();

            ReportResults(runner);
        }

        private static void ReportResults(TestsRunner runner)
        {
            StringBuilder header = new StringBuilder();

            header.AppendLine().AppendLine().AppendLine().AppendLine()
                .AppendLine(delimiter).AppendLine()
                .AppendLine($"Tests run {runner.RunStatus}").AppendLine();

            var color = runner.RunStatus.Equals(Result.Passed) ? ConsoleColor.Green : ConsoleColor.Red;
            Console.ForegroundColor = color;

            Console.Write(header.ToString());

            int passedTests = 0;
            int skippedTests = 0;
            int failedTests = 0;

            foreach (var suite in runner.ExecutedSuites)
            {
                failedTests += suite.Outcome.FailedTests;
                skippedTests += suite.Outcome.SkippedTests;
                passedTests += suite.Outcome.TotalTests - suite.Outcome.FailedTests - suite.Outcome.SkippedTests;
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Passed tests: {passedTests}    Skipped tests: {skippedTests}    Failed tests: {failedTests}");
        }

        private static void ReportHeader(string assemblyPath)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            Console.WriteLine(ResourceAsciiLogo.Logo);
            Console.WriteLine();

            Console.WriteLine(delimiter);
            Console.WriteLine();
            Console.WriteLine("Configuration");
            Console.WriteLine();
            Console.WriteLine("Tests assembly: " + assemblyPath);
            Console.WriteLine(Configuration.GetInfo());
            Console.WriteLine(delimiter);
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void PrintHelpText()
        {
            Console.WriteLine("Please specify necessary parameters to run:");
            Console.WriteLine($"{ConstTestAssembly}=<test_assembly_path>");
            Console.WriteLine($"{ConstConfiguration}=<configuration_file_path>");
        }
    }
}
