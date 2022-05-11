using P2PLending.LoanMonitor.Core.Enums;
using System.Collections.Generic;

namespace P2PLending.LoanMonitor.Core.Models
{
    public class LoanListing
    {
        public LoanIssuer Issuer { get; set; }
        public IEnumerable<Loan> Loans { get; set; }
    }
}