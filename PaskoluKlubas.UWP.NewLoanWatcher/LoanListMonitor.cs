using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaskoluKlubas.UWP.NewLoanWatcher
{
    public class LoanListMonitor
    {
        private readonly IEnumerable<ILoanIssuerClient> _loanIssuers;

        private Dictionary<LoanIssuer, IEnumerable<Loan>> _currentLoanListing = new Dictionary<LoanIssuer, IEnumerable<Loan>>();

        public LoanListMonitor(IEnumerable<ILoanIssuerClient> loanIssuers)
        {
            _loanIssuers = loanIssuers;
        }

        public  async Task<IDictionary<LoanIssuer, IEnumerable<Loan>>> GetNewLoansAsync()
        {
            var newLoans = new Dictionary<LoanIssuer, IEnumerable<Loan>>();
            
            foreach (var loanIssuer in _loanIssuers)
            {
                var newLoanListing = await loanIssuer.GetLoanListingAsync();

                if (_currentLoanListing.ContainsKey(loanIssuer.Name))
                {
                    var delta = newLoanListing.Where(x => !_currentLoanListing[loanIssuer.Name].Contains(x));

                    if (delta.Any())
                    {
                        newLoans[loanIssuer.Name] = delta;
                    }

                    _currentLoanListing[loanIssuer.Name] = newLoanListing;
                    continue;
                }

                _currentLoanListing.Add(loanIssuer.Name, newLoanListing);
            }

            return newLoans;
        }
    }
}
