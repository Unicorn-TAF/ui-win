using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Unicorn.Core.Logging;

namespace Unicorn.Core.Utility
{
    public class FileDownloader
    {
        private readonly string destinationFolder;
        private string downloadFileName;
        private string[] fileNamesToExclude;
        private HashSet<string> fileNamesBeforeDownload;
        private TimeSpan pollingInterval = TimeSpan.FromMilliseconds(500);

        public FileDownloader(string destinationFolder)
        {
            this.destinationFolder = destinationFolder;
            this.downloadFileName = null;
            this.fileNamesToExclude = null;
            this.fileNamesBeforeDownload = GetFileNamesFromDestinationFolder();
        }

        public FileDownloader(string destinationFolder, string downloadFileName)
        {
            this.downloadFileName = downloadFileName;
            this.fileNamesToExclude = null;
            this.fileNamesBeforeDownload = null;
        }

        public void ExcludeFileNames(params string[] fileNames)
        {
            this.fileNamesToExclude = fileNames;
        }

        public string WaitForFileToBeDownloaded(TimeSpan timeout)
        {
            if (!string.IsNullOrEmpty(downloadFileName))
            {
                return WaitForExpectedFile(timeout);
            }
            else
            {
                return WaitForFileAllocation(timeout);
            }
        }

        private string WaitForFileAllocation(TimeSpan timeout)
        {
            var timer = Countdown.StartNew(timeout);
            bool downloading = true;
            string allocatedFile = null;

            do
            {
                var currentFiles = GetFileNamesFromDestinationFolder();
                currentFiles.ExceptWith(fileNamesBeforeDownload);

                if (fileNamesToExclude != null)
                {
                    var excludes = new List<string>();
                    
                    foreach (var file in fileNamesToExclude)
                    {
                        excludes.AddRange(currentFiles.Where(f => f.EndsWith(file)));
                    }

                    currentFiles.ExceptWith(excludes);
                }

                if (currentFiles.Count > 1)
                {
                    throw new FileNotFoundException($"Unable to allocate file as {currentFiles.Count} files found.");
                }

                if (currentFiles.Any())
                {
                    allocatedFile = currentFiles.First();
                    downloading = false;
                }

                Thread.Sleep(pollingInterval);
            }
            while (downloading && !timer.Expired) ;

            timer.ThrowExceptionIfExpired($"File was not downloaded in time");

            return allocatedFile;
        }

        private string WaitForExpectedFile(TimeSpan timeout)
        {
            var timer = Countdown.StartNew(timeout);

            do
            {
                Thread.Sleep(pollingInterval);
            }
            while (!File.Exists(downloadFileName) && !timer.Expired) ;

            timer.ThrowExceptionIfExpired($"File '{downloadFileName}' was not downloaded in time");

            return downloadFileName;
        }

        //find all files from download
        private HashSet<string> GetFileNamesFromDestinationFolder()
        {
            Logger.Instance.Log(LogLevel.Debug, "Get files list from Downloads");
            return new HashSet<string>(Directory.GetFiles(this.destinationFolder));
        }
    }
}
