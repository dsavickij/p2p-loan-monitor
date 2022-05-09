using System.Collections.Generic;

namespace PaskoluKlubas.UWP.NewLoanWatcher
{
    public class LoanListing
    {
        public LoanIssuer Issuer { get; set; }
        public IEnumerable<Loan> Loans { get; set; }
    }
}