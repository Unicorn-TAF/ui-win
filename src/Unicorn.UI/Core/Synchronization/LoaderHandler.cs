using System;
using System.Diagnostics;
using System.Threading;
using Unicorn.Taf.Core.Logging;

namespace Unicorn.UI.Core.Synchronization
{
    /// <summary>
    /// Describes handler for loading process (when some loader control appears first and then disappears after loading is complete).
    /// </summary>
    public class LoaderHandler
    {
        private const string Prefix = nameof(LoaderHandler) + ": ";

        private readonly Func<bool> _appearanceCondition;
        private readonly Func<bool> _disappearanceCondition;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoaderHandler"/> class with specified appearance and disappearance conditions.
        /// </summary>
        /// <param name="appearanceCondition"></param>
        /// <param name="disappearanceCondition"></param>
        public LoaderHandler(Func<bool> appearanceCondition, Func<bool> disappearanceCondition)
        {
            _appearanceCondition = appearanceCondition;
            _disappearanceCondition = disappearanceCondition;
        }

        /// <summary>
        /// Performs standard loading process waiting with specified polling interval:<para/>
        /// - Waits for loader appearance condition is met during specified timeout<para/>
        /// - Waits for loader disappearance condition is met during specified timeout
        /// </summary>
        /// <param name="appearanceTimeout">time to wait for loader appearance</param>
        /// <param name="disappearanceTimeout">time to wait for loader disappearance</param>
        /// <param name="pollingInterval">polling interval between status checks</param>
        /// <exception cref="LoaderTimeoutException">is thrown if loader is still displayed after timeout expiration</exception>
        public void WaitFor(TimeSpan appearanceTimeout, TimeSpan disappearanceTimeout, TimeSpan pollingInterval)
        {
            Logger.Instance.Log(LogLevel.Debug, $"{Prefix}Start");

            var timer = Stopwatch.StartNew();
            bool loaderAppeared;

            // Wait for loader appearance during appearance timeout.
            do
            {
                loaderAppeared = _appearanceCondition.Invoke();
                Thread.Sleep(pollingInterval);
            }
            while (timer.Elapsed < appearanceTimeout && !loaderAppeared);

            Logger.Instance.Log(LogLevel.Debug, $"\t{Prefix}appeared - {loaderAppeared} : {timer.ElapsedMilliseconds} ms.");

            // Wait for loader disappearance during disappearance timeout.
            bool loaderDisappeared;
            do
            {
                loaderDisappeared = _disappearanceCondition.Invoke();
                Thread.Sleep(pollingInterval);
            }
            while (timer.Elapsed < disappearanceTimeout && !loaderDisappeared);

            timer.Stop();

            Logger.Instance.Log(LogLevel.Debug, $"\t{Prefix}disappeared - {loaderDisappeared} : {timer.ElapsedMilliseconds} ms.");

            if (!loaderDisappeared)
            {
                throw new LoaderTimeoutException($"Loader has not disappeared in {disappearanceTimeout.ToString(@"mm\:ss\.fff")}.");
            }
        }

        /// <summary>
        /// Performs standard loading process waiting with 250 ms polling interval:<para/>
        /// - Waits for loader appearance condition is met during specified timeout<para/>
        /// - Waits for loader disappearance condition is met during specified timeout
        /// </summary>
        /// <param name="appearanceTimeout">time to wait for loader appearance</param>
        /// <param name="disappearanceTimeout">time to wait for loader disappearance</param>
        /// <exception cref="LoaderTimeoutException">is thrown if loader is still displayed after timeout expiration</exception>
        public void WaitFor(TimeSpan appearanceTimeout, TimeSpan disappearanceTimeout) =>
            WaitFor(appearanceTimeout, disappearanceTimeout, TimeSpan.FromMilliseconds(250));

        /// <summary>
        /// Performs standard loading process waiting with 250 ms polling interval:<para/>
        /// - Waits for loader appearance condition is met during 1.5 seconds<para/>
        /// - Waits for loader disappearance condition is met during specified timeout
        /// </summary>
        /// <param name="disappearanceTimeout">time to wait for loader disappearance</param>
        /// <exception cref="LoaderTimeoutException">is thrown if loader is still displayed after timeout expiration</exception>
        public void WaitFor(TimeSpan disappearanceTimeout) =>
            WaitFor(TimeSpan.FromSeconds(1.5), disappearanceTimeout, TimeSpan.FromMilliseconds(250));
    }
}
