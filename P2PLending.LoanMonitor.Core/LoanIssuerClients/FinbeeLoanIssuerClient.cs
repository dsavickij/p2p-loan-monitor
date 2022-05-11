using P2PLending.LoanMonitor.Core.Enums;
using P2PLending.LoanMonitor.Core.LoanIssuerClients.Abstractions;
using P2PLending.LoanMonitor.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace P2PLending.LoanMonitor.Core.LoanIssuerClients
{
    public class FinbeeLoanIssuerClient : ILoanIssuerClient
    {
        private readonly string _login;
        private readonly string _password;

        public LoanIssuer Issuer => LoanIssuer.Finbee;

        public FinbeeLoanIssuerClient(string login, string password)
        {
            _login = login;
            _password = password;
        }

        // will be added eventually
        public async Task<IEnumerable<Loan>> GetLoanListingAsync()
        {
            return new[] { new Loan { } };
        }
    }
}
