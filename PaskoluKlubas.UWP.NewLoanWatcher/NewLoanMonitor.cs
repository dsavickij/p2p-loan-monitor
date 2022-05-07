using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaskoluKlubas.UWP.NewLoanWatcher
{
    public class NewLoanMonitor
    {
        private readonly IEnumerable<ILoanIssuer> _loanIssuers;
        private readonly IEnumerable<LoanIssuerLoginSettings> _loginSettings;

        private Dictionary<LoanIssuer, IEnumerable<Loan>> _currentLoanListing = new Dictionary<LoanIssuer, IEnumerable<Loan>>();

        public NewLoanMonitor(IEnumerable<ILoanIssuer> loanIssuers, IEnumerable<LoanIssuerLoginSettings> loginSettings)
        {
            _loanIssuers = loanIssuers;
            _loginSettings = loginSettings;
        }

        public  async Task<IDictionary<LoanIssuer, IEnumerable<Loan>>> GetIssuedNewLoansAsync()
        {
            var newLoans = new Dictionary<LoanIssuer, IEnumerable<Loan>>();
            
            foreach (var loanIssuer in _loanIssuers)
            {
                var loginSettings = _loginSettings.FirstOrDefault(x => x.LoanIssuer == loanIssuer.LoanIssuer);

                if (loginSettings == null)
                    continue;

                var newLoanListing = await loanIssuer.GetLoanListingAsync(loginSettings.Login, loginSettings.Password);

                if (_currentLoanListing.ContainsKey(loginSettings.LoanIssuer))
                {
                    var delta = newLoanListing.Where(x => !_currentLoanListing[loginSettings.LoanIssuer].Contains(x));

                    if (delta.Any())
                    {
                        newLoans[loginSettings.LoanIssuer] = delta;
                    }

                    _currentLoanListing[loginSettings.LoanIssuer] = newLoanListing;
                    continue;
                }

                _currentLoanListing.Add(loginSettings.LoanIssuer, newLoanListing);
            }

            return newLoans;
        }
    }
}
