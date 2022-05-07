using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaskoluKlubas.UWP.NewLoanWatcher
{
    public class FinbeeLoanIssuer : ILoanIssuer
    {
        public LoanIssuer LoanIssuer => LoanIssuer.Finbee;

        public async Task<IEnumerable<Loan>> GetLoanListingAsync(string login, string password)
        {
            return new[] { new Loan { } };
        }
    }
}
