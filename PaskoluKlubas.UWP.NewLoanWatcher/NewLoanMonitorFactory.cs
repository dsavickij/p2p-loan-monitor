using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaskoluKlubas.UWP.NewLoanWatcher
{
    public class NewLoanMonitorFactory
    {
        private readonly IEnumerable<ILoanIssuer> _loanIssuers;

        public NewLoanMonitorFactory(IEnumerable<ILoanIssuer> loanIssuers)
        {
            _loanIssuers = loanIssuers;
        }

        public NewLoanMonitor Create(IEnumerable<LoanIssuerLoginSettings> cfgs)
        {
            if (cfgs.GroupBy(x => x.LoanIssuer).Any(x => x.Count() > 1))
            {
                throw new Exception("More than one settings instance for the same loan issuer");
            }
                   
            return new NewLoanMonitor(_loanIssuers, cfgs);
        }


        public NewLoanMonitor Create(Action<IEnumerable<LoanIssuerLoginSettings>> configure)
        {
            var cfgs = new List<LoanIssuerLoginSettings>();
            configure(cfgs);

            return new NewLoanMonitor(_loanIssuers, cfgs);
        }
    }
}
