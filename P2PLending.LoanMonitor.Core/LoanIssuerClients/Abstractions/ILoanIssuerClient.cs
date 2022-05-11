using System.Collections.Generic;
using System.Threading.Tasks;
using P2PLending.LoanMonitor.Core.Enums;
using P2PLending.LoanMonitor.Core.Models;

namespace P2PLending.LoanMonitor.Core.LoanIssuerClients.Abstractions
{
    public interface ILoanIssuerClient
    {
        LoanIssuer Issuer { get; }

        Task<IEnumerable<Loan>> GetLoanListingAsync();
    }
}