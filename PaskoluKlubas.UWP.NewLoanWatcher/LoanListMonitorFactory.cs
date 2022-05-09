using PaskoluKlubas.UWP.NewLoanWatcher.LoanIssuerClients.PaskoluKlubas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaskoluKlubas.UWP.NewLoanWatcher
{
    public class LoanListMonitorFactory
    {
        public static LoanListMonitor Create(LoanIssuerClientConfiguration cfg)
        {
            ILoanIssuerClient client;
            
            switch (cfg.LoanIssuer)
            {
                case LoanIssuer.PaskoluKlubas:
                    client = new PKLoanIssuerClient(cfg.Login, cfg.Password);
                    break;
                case LoanIssuer.Finbee:
                    client = new FinbeeLoanIssuerClient(cfg.Login, cfg.Password);
                    break;
                default:
                    throw new Exception($"Loan issuer '{cfg.LoanIssuer}' is not supported");
            }

            return new LoanListMonitor(client);
        }
    }
}
