//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PaskoluKlubas.UWP.NewLoanWatcher
//{
//    public class FinbeeLoanIssuerClient : ILoanIssuerClient
//    {
//        private readonly string _login;
//        private readonly string _password;

//        public LoanIssuer Issuer => LoanIssuer.Finbee;

//        public FinbeeLoanIssuerClient(string login, string password)
//        {
//            _login = login;
//            _password = password;
//        }

//        // will be added eventually
//        public async Task<IEnumerable<Loan>> GetLoanListingAsync()
//        {
//            return new[] { new Loan { } };
//        }
//    }
//}
