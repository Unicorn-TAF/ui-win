using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Unicorn.Taf.Core.Utility;

namespace Unicorn.UnitTests.Core.Utility
{
    [TestFixture]
    public class FileAllocatorFunctionality
    {
        private const string TestFile = "FileAllocatorTestFile";
        private const string TestFile2 = "TestFileForAllocator";
        private const string IgnoreFile = "FileAllocatorIgnoreFile";
        private static string testsPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static string testFileFullPath = Path.Combine(testsPath, TestFile);
        private static string testFileFullPath2 = Path.Combine(testsPath, TestFile2);
        private static string ignoreFileFullPath = Path.Combine(testsPath, IgnoreFile);

        [OneTimeTearDown]
        public static void Cleanup()
        {
            File.Delete(testFileFullPath);
            File.Delete(testFileFullPath2);
            File.Delete(ignoreFileFullPath);
            testsPath = null;
            testFileFullPath = null;
            ignoreFileFullPath = null;
            testFileFullPath2 = null;
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestFileAllocationWithKnownExistingFile()
        {
            var assemblyFile = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
            var fileName = new FileAllocator(testsPath, assemblyFile).WaitForFileToAppear(TimeSpan.FromMilliseconds(100));
            Assert.That(fileName, Is.EqualTo(assemblyFile));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestFileAllocationWithKnownExpectedPartOfExistingFile()
        {
            var allocator = new FileAllocator(testsPath)
            {
                ExpectedFileNamePart = "stFileForAllo"
            };
            string fileName = null;

            var task = new Task(() => fileName = allocator.WaitForFileToAppear(TimeSpan.FromSeconds(1)));

            task.Start();
            File.Create(testFileFullPath2).Close();
            task.Wait();

            Assert.That(fileName, Is.EqualTo(TestFile2));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestFileAllocationWithKnownNotExistingFile() =>
             Assert.Throws<TimeoutException>(delegate
             {
                 new FileAllocator(testsPath, "someNotExistingFile").WaitForFileToAppear(TimeSpan.FromMilliseconds(200));
             });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestFileAllocationFilesIgnoring()
        {
            var allocator = new FileAllocator(testsPath);
            allocator.ExcludeFileNames("FileAllocatorIgnoreFile");

            var task = new Task(() => allocator.WaitForFileToAppear(TimeSpan.FromSeconds(1)));

            try
            {
                task.Start();
                File.Create(ignoreFileFullPath).Close();
                task.Wait();
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException.GetType().Equals(typeof(TimeoutException)))
                {
                    return;
                }
            }

            throw new AssertionException("File was allocated but shouldn't");
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestFileAllocationExpectedFileAppearance()
        {
            var allocator = new FileAllocator(testsPath);
            allocator.ExcludeFileNames("FileAllocatorIgnoreFile");
            string fileName = null;

            var task = new Task(() => fileName = allocator.WaitForFileToAppear(TimeSpan.FromSeconds(1)));

            task.Start();
            File.Create(testFileFullPath).Close();
            File.Create(ignoreFileFullPath).Close();
            task.Wait();

            Assert.That(fileName, Is.EqualTo(TestFile));
        }
    }
}
