using P2PLending.LoanMonitor.Core.Enums;
using P2PLending.LoanMonitor.Core.LoanIssuerClients;
using P2PLending.LoanMonitor.Core.LoanIssuerClients.Abstractions;
using P2PLending.LoanMonitor.Core.Models;
using System;

namespace P2PLending.LoanMonitor.Core
{
    public class LoanListMonitorFactory
    {
        public static LoanListMonitor Create(LoanIssuerClientConfiguration cfg)
        {
            ILoanIssuerClient client;
            
            switch (cfg.LoanIssuer)
            {
                case LoanIssuer.PaskoluKlubas:
                    client = new PaskoluKlubasLoanIssuerClient(cfg.Login, cfg.Password);
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
