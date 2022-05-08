using PaskoluKlubas.UWP.NewLoanWatcher.LoanIssuerClients.PaskoluKlubas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaskoluKlubas.UWP.NewLoanWatcher
{
    public class LoanListMonitorFactory
    {
        public static LoanListMonitor Create(IEnumerable<LoanIssuerClientConfiguration> cfgs)
        {
            if (cfgs.GroupBy(x => x.LoanIssuer).Any(x => x.Count() > 1))
            {
                throw new Exception("More than one configuration is for the same loan issuer");
            }
               
            var loanIssuers = new List<ILoanIssuerClient>();

            foreach (var cfg in cfgs)
            {
                switch (cfg.LoanIssuer)
                {
                    case LoanIssuer.PaskoluKlubas:
                        loanIssuers.Add(new PKLoanIssuerClient(cfg.Login, cfg.Password));
                        break;
                    case LoanIssuer.Finbee:
                        loanIssuers.Add(new FinbeeLoanIssuerClient(cfg.Login, cfg.Password));
                        break;
                    default:
                        throw new Exception($"Loan issuer {cfg.LoanIssuer} is not supported");
                }
            }

            return new LoanListMonitor(loanIssuers);
        }
    }
}
