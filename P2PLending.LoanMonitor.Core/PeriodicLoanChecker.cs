﻿using P2PLending.LoanMonitor.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace P2PLending.LoanMonitor.Core
{
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

        public async Task StartAsync()
        {
            _keepChecking = true;

            while (_keepChecking)
            {
                var listing = await _monitor.GetNewListedLoansAsync();

                if (listing.Loans.Any())
                {
                    _handler.Invoke(listing);
                }

                await Task.Delay(_timeSpan);
            };
        }

        public void Stop()
        {
            _keepChecking = false;
        }
    }
}
