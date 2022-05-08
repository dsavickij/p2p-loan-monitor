using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaskoluKlubas.UWP.NewLoanWatcher.LoanIssuerClients.PaskoluKlubas
{
    public partial class PKLoanIssuerClient : ILoanIssuerLoginCheck
    {
        public static ILoanIssuerLoginCheck WithCredentials(string login, string password)
        {
            return new PKLoanIssuerClient(login, password);
        }

        public async Task<bool> IsAbleToLoginAsync()
        {
            using (var client = GetWebPageClientAsync())
            {
                var loginFormDoc = await GetLoginFormPageAsync(client);
                await AcceptCookiesAsync(client, loginFormDoc);

                var content = GetLoginPageFormUrlEncodedContent(loginFormDoc);
                var postLoginDoc = await ExecuteLoginAsync(client, content);

                return IsLoginSuccessful(postLoginDoc);
            }
        }

        private bool IsLoginSuccessful(HtmlDocument postLoginDoc) =>
            postLoginDoc.Text.Contains(@"https:\/\/www.paskoluklubas.lt\/user");
    }
}
