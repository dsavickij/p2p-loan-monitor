using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaskoluKlubas.UWP.NewLoanWatcher
{
    public class LoanListMonitor
    {
        private readonly ILoanIssuerClient _loanIssuerClient;

        private LoanListing _currentLoanListing = new LoanListing();

        public LoanListMonitor(ILoanIssuerClient loanIssuer)
        {
            _loanIssuerClient = loanIssuer;
        }

        public async Task<LoanListing> GetNewListedLoansAsync()
        {
            var newloanListing = new LoanListing { Issuer = _loanIssuerClient.Issuer, Loans = Enumerable.Empty<Loan>() };

            var loanListing = await _loanIssuerClient.GetLoanListingAsync();

            if (_currentLoanListing.Loans != null)
            {
                var delta = loanListing.Where(x => !_currentLoanListing.Loans.Contains(x)).ToList();
                newloanListing.Loans = delta;
            }

            _currentLoanListing.Loans = loanListing;

            return newloanListing;
        }
    }
}
