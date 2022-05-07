using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using PaskoluKlubas.UWP.NewLoanWatcher;

public interface ILoanIssuer
{
    LoanIssuer LoanIssuer { get; }
    
    Task<IEnumerable<Loan>> GetLoanListingAsync(string login, string password);
}

public interface ICredentialsCheck
{
    Task<bool> IsAbleToLoginAsync(string login, string password);
}

public class PaskoluKlubasLoanIssuer : ILoanIssuer, ICredentialsCheck
{
    public LoanIssuer LoanIssuer => LoanIssuer.PaskoluKlubas;

    public async Task<IEnumerable<Loan>> GetLoanListingAsync(string login, string password)
    {
        using (var client = GetWebPageClientAsync())
        {
            var loginFormDoc = await GetLoginFormPageAsync(client);
            await AcceptCookiesAsync(client, loginFormDoc);

            var csfrToken = GetCsfrToken(loginFormDoc);
            var content = GetLoginPageFormUrlEncodedContent(csfrToken, login, password);
            var postLoginDoc = await ExecuteLoginAsync(client, content);

            if (IsLoginSuccessful(postLoginDoc))
            {
                var loanDoc = await GetLoanPageAsync(client);
                return ParseLoans(loanDoc);
            }

            throw new Exception("");
        }
    }

    private IEnumerable<Loan> ParseLoans(HtmlDocument loanDoc)
    {
        var loans = new List<Loan>();

        var rows = loanDoc.GetElementbyId("primary_loans_for_investor_list").SelectNodes("table[1]/tbody[1]/tr");

        foreach (var row in rows)
        {
            loans.Add(new Loan
            {
                Id = row.SelectNodes("td[1]/div[1]/a[1]/span").First().InnerText,
                CreditRating = row.SelectNodes("td[2]/span").First().InnerText.Trim(new[] { ' ', '\n', '\r' }),
                InterestRate = double.Parse(row.SelectNodes("td[3]/div/div").First().InnerText.Trim(new[] { '%' })),
                Duration = int.Parse(row.SelectNodes("td[4]/div").First().InnerText.Split(' ')[0]),
                Amount = int.Parse(row.SelectNodes("td[5]/div").First().InnerText.Split('.')[0])
            });
        }

        return loans;
    }

    private async Task<HtmlDocument> GetLoanPageAsync(HttpClient client)
    {
        var loanListResult = await client.GetAsync("/investor/investment/list/loan/requests");
        var stream = await loanListResult.Content.ReadAsStreamAsync();

        var doc = new HtmlDocument();
        doc.Load(stream);

        return doc;
    }

    public async Task<bool> IsAbleToLoginAsync(string login, string password)
    {
        using (var client = GetWebPageClientAsync())
        {
            var loginFormDoc = await GetLoginFormPageAsync(client);
            await AcceptCookiesAsync(client, loginFormDoc);

            var csfrToken = GetCsfrToken(loginFormDoc);
            var content = GetLoginPageFormUrlEncodedContent(csfrToken, login, password);
            var postLoginDoc = await ExecuteLoginAsync(client, content);

            return IsLoginSuccessful(postLoginDoc);
        }
    }

    private bool IsLoginSuccessful(HtmlDocument postLoginDoc) =>
        postLoginDoc.Text.Contains(@"https:\/\/www.paskoluklubas.lt\/user");

    private string GetCsfrToken(HtmlDocument loginDoc)
    {
        var csrfTokenElement = loginDoc.GetElementbyId("_csrf_token");
        return csrfTokenElement.GetAttributes("value").FirstOrDefault().Value;
    }

    private FormUrlEncodedContent GetLoginPageFormUrlEncodedContent(string csfrToken, string login, string password)
    {
        return new FormUrlEncodedContent(new[]
        {
            //the name of the form values must be the name of <input />
            //tags of the login form, in this case the tag is <input type="text" name="username">
            new KeyValuePair<string, string>("_username", login),
            new KeyValuePair<string, string>("_password", password),
            new KeyValuePair<string, string>("_csrf_token", csfrToken),
            new KeyValuePair<string, string>("step", "first"),
        });
    }

    private async Task<HtmlDocument> ExecuteLoginAsync(HttpClient client, FormUrlEncodedContent content)
    {
        var login = await client.PostAsync("/prisijungti", content);
        login.EnsureSuccessStatusCode();

        var stream = await login.Content.ReadAsStreamAsync();

        var doc = new HtmlDocument();
        doc.Load(stream);

        return doc;
    }

    private async Task AcceptCookiesAsync(HttpClient client, HtmlDocument loginDoc)
    {
        var cookieConsent = loginDoc.GetElementbyId("cookie_consent__token");
        var ccvalue = cookieConsent.GetAttributes("value").FirstOrDefault().Value;

        var consentContent = new FormUrlEncodedContent(new[]
        {
                new KeyValuePair<string, string>("cookie_consent[_token]", ccvalue)
        });

        var consentResult = await client.PostAsync("/cookie/consent", consentContent);
        consentResult.EnsureSuccessStatusCode();
    }

    private async Task<HtmlDocument> GetLoginFormPageAsync(HttpClient client)
    {
        var loginGetResult = await client.GetAsync("/prisijungti");
        var loginStream = await loginGetResult.Content.ReadAsStreamAsync();

        var doc = new HtmlDocument();
        doc.Load(loginStream);

        return doc;
    }

    private HttpClient GetWebPageClientAsync()
    {
        var baseAddress = new Uri("https://www.paskoluklubas.lt/");

        var handler = new HttpClientHandler()
        {
            CookieContainer = new CookieContainer()
        };

        return new HttpClient(handler) { BaseAddress = baseAddress };
    }
}
