using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using ReportPortal.UnicornExtension.EventArguments;
using ReportPortal.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unicorn.Core.Testing.Tests;

namespace ReportPortal.UnicornExtension
{
    public partial class ReportPortalListener
    {
        public delegate void SuiteStartedHandler(object sender, TestItemStartedEventArgs e);
        public static event SuiteStartedHandler BeforeSuiteStarted;
        public static event SuiteStartedHandler AfterSuiteStarted;

        private void StartSuite(TestSuite suite)
        {
            try
            {
                var id = suite.Id.ToString();
                var parentId = "";
                var name = suite.Name;

                var startSuiteRequest = new StartTestItemRequest
                {
                    StartTime = DateTime.UtcNow,
                    Name = name,
                    Type = TestItemType.Suite
                };

                var beforeSuiteEventArg = new TestItemStartedEventArgs(Bridge.Service, startSuiteRequest);
                try
                {
                    if (BeforeSuiteStarted != null) BeforeSuiteStarted(this, beforeSuiteEventArg);
                }
                catch (Exception exp)
                {
                    Console.WriteLine("Exception was thrown in 'BeforeSuiteStarted' subscriber." + Environment.NewLine + exp);
                }
                if (!beforeSuiteEventArg.Canceled)
                {
                    TestReporter test;
                    if (string.IsNullOrEmpty(parentId) || !_suitesFlow.ContainsKey(parentId))
                    {
                        test = Bridge.Context.LaunchReporter.StartNewTestNode(startSuiteRequest);
                    }
                    else
                    {
                        test = _suitesFlow[parentId].StartNewTestNode(startSuiteRequest);
                    }

                    _suitesFlow[id] = test;

                    try
                    {
                        if (AfterSuiteStarted != null) AfterSuiteStarted(this, new TestItemStartedEventArgs(Bridge.Service, startSuiteRequest, test));
                    }
                    catch (Exception exp)
                    {
                        Console.WriteLine("Exception was thrown in 'AfterSuiteStarted' subscriber." + Environment.NewLine + exp);
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }

        public delegate void SuiteFinishedHandler(object sender, TestItemFinishedEventArgs e);
        public static event SuiteFinishedHandler BeforeSuiteFinished;
        public static event SuiteFinishedHandler AfterSuiteFinished;

        private void FinishSuite(TestSuite suite)
        {
            try
            {
                var id = suite.Id.ToString();
                var result = suite.Outcome.Result.ToString();
                var parentId = "";

                // at the end of execution nunit raises 2 the same events, we need only that which has 'parentId' xml tag
                if (parentId != null)
                {
                    if (_suitesFlow.ContainsKey(id))
                    {
                        var updateSuiteRequest = new UpdateTestItemRequest();

                        // adding categories to suite
                        var categories = suite.Features;
                        if (categories != null)
                        {
                            updateSuiteRequest.Tags = new List<string>();

                            foreach (string category in categories)
                            {
                                updateSuiteRequest.Tags.Add(category);
                            }
                        }

                        // adding description to suite
                        var description = suite.Name;
                        if (description != null)
                        {
                            updateSuiteRequest.Description = description;
                        }

                        if (updateSuiteRequest.Description != null || updateSuiteRequest.Tags != null)
                        {
                            _suitesFlow[id].AdditionalTasks.Add(Task.Run(() =>
                            {
                                _suitesFlow[id].StartTask.Wait();
                                Bridge.Service.UpdateTestItem(_suitesFlow[id].TestId, updateSuiteRequest);
                            }));
                        }

                        // finishing suite
                        var finishSuiteRequest = new FinishTestItemRequest
                        {
                            EndTime = DateTime.UtcNow,
                            Status = _statusMap[result]
                        };
                        
                        var eventArg = new TestItemFinishedEventArgs(Bridge.Service, finishSuiteRequest, _suitesFlow[id]);

                        try
                        {
                            if (BeforeSuiteFinished != null) BeforeSuiteFinished(this, eventArg);
                        }
                        catch (Exception exp)
                        {
                            Console.WriteLine("Exception was thrown in 'BeforeSuiteFinished' subscriber." + Environment.NewLine + exp);
                        }

                        _suitesFlow[id].Finish(finishSuiteRequest);

                        try
                        {
                            if (AfterSuiteFinished != null) AfterSuiteFinished(this, new TestItemFinishedEventArgs(Bridge.Service, finishSuiteRequest,_suitesFlow[id]));
                        }
                        catch (Exception exp)
                        {
                            Console.WriteLine("Exception was thrown in 'AfterSuiteFinished' subscriber." + Environment.NewLine + exp);
                        }
                    }
                }
            }
            catch(Exception exception)
            {
                Console.WriteLine("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }
    }
}