using System.Collections.Generic;
using System.Threading.Tasks;
using PaskoluKlubas.UWP.NewLoanWatcher;

public interface ILoanIssuerClient
{
    LoanIssuer Name { get; }

    Task<IEnumerable<Loan>> GetLoanListingAsync();
}
