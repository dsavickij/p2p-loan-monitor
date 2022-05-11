using P2PLending.LoanMonitor.Core.Enums;

namespace P2PLending.LoanMonitor.Core.Models
{
    public class LoanIssuerClientConfiguration
    {
        public LoanIssuer LoanIssuer { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }
    }
}