﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Documents;

namespace PaskoluKlubas.UWP.NewLoanWatcher
{
    public class PeriodicLoanCheckerBuilder : IPeriodicLoanCheckerBuilder, IPeriodicLoanCheckerBuilderTimespanSetup, IPeriodicLoanCheckerBuilderHandlerSetup
    {
        private readonly LoanListMonitor _monitor;
        private readonly TimeSpan _timeSpan;
        private readonly Action<LoanListing> _handler;
        
        private PeriodicLoanCheckerBuilder(
            LoanListMonitor monitor,
            TimeSpan timeSpan,
            Action<LoanListing> handler) : this(monitor, timeSpan)
        {
            _handler = handler;
        }

        private PeriodicLoanCheckerBuilder(LoanListMonitor monitor, TimeSpan timeSpan) : this(monitor)
        {
            _timeSpan = timeSpan;
        }

        private PeriodicLoanCheckerBuilder(LoanListMonitor monitor)
        {
            _monitor = monitor;
        }
        
        public static IPeriodicLoanCheckerBuilderTimespanSetup SetLoanIssuer(LoanIssuerClientConfiguration cfg)
        {
            var monitor = LoanListMonitorFactory.Create(cfg);
            
            return new PeriodicLoanCheckerBuilder(monitor);
        }

        public IPeriodicLoanCheckerBuilderHandlerSetup CheckEvery(TimeSpan span)
        {
            return new PeriodicLoanCheckerBuilder(_monitor, span);
        }

        public PeriodicLoanChecker Build()
        {
            return new PeriodicLoanChecker(_monitor, _timeSpan, _handler);
        }

        public IPeriodicLoanCheckerBuilder CallOnNewLoans(Action<LoanListing> handler)
        {
            return new PeriodicLoanCheckerBuilder(_monitor, _timeSpan, handler);
        }
    }

    public class PeriodicLoanChecker
    {
        private readonly LoanListMonitor _monitor;
        private readonly TimeSpan _timeSpan;
        private readonly Action<LoanListing> _handler;

        private bool _keepChecking;

        public PeriodicLoanChecker(
            LoanListMonitor monitor,
            TimeSpan timeSpan,
            Action<LoanListing> handler)
        {
            _monitor = monitor;
            _timeSpan = timeSpan;
            _handler = handler;
        }

        public void Start()
        {
            _keepChecking = true;

            Task.Run(async () =>
            {                
                while (_keepChecking)
                {
                    var listing = await _monitor.GetNewLoanListingAsync();

                    if (listing.Loans.Any())
                    {
                        _handler.Invoke(listing);
                    }

                    await Task.Delay(_timeSpan);
                }
            }).ContinueWith(x =>
            {
                if (x.IsFaulted)
                {
                    throw x.Exception;
                }
            });
        }

        public void Stop()
        {
            _keepChecking = false;
        }
    }

    //public class PeriodicLoanChecker2 : IPeriodicLoanCheckerLoanIssuerSetup, IPeriodicLoanCheckerTimeSpanSetup
    //{
    //    private readonly LoanListMonitor _monitor;
    //    private readonly TimeSpan _timeSpan;

    //    private bool _keepChecking;



    //    public void Start()
    //    {

    //    }

    //    public void Stop()
    //    {

    //    }
    //}


    //public interface IPeriodicLoanCheckerTimeSpanSetup
    //{
    //    PeriodicLoanChecker2 Every(TimeSpan span);
    //}

    //public interface IPeriodicLoanCheckerLoanIssuerSetup
    //{
    //    IPeriodicLoanCheckerTimeSpanSetup SetLoanIssuers(IEnumerable<LoanIssuerClientConfiguration> clientCfgs);
    //}


    public interface IPeriodicLoanCheckerBuilder
    {
        PeriodicLoanChecker Build();
    }

    public interface IPeriodicLoanCheckerBuilderTimespanSetup
    {
        IPeriodicLoanCheckerBuilderHandlerSetup CheckEvery(TimeSpan span);
    }

    public interface IPeriodicLoanCheckerBuilderHandlerSetup
    {
        IPeriodicLoanCheckerBuilder CallOnNewLoans(Action<LoanListing> handler);
    }
}