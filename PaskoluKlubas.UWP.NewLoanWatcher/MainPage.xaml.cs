using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using PaskoluKlubas.UWP.NewLoanWatcher.LoanIssuerClients.PaskoluKlubas;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PaskoluKlubas.UWP.NewLoanWatcher
{


    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly IServiceProvider _serviceProvider;
        private PeriodicLoanChecker _loanChecker;

        public MainPage()
        {
            this.InitializeComponent();

            _serviceProvider = RegisterDependencies();
        }

        private IServiceProvider RegisterDependencies()
        {
            var services = new ServiceCollection();



            return services.BuildServiceProvider();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            watcher_start.IsEnabled = false;

            var toastMessenger = new PaskoluKlubasToastMessageRenderer();

            toastMessenger.ShowToastMessage(new LoanListing
            {
                Issuer = LoanIssuer.PaskoluKlubas,
                Loans = new[]
                {
                    new Loan { Amount = 5, CreditRating = "B", Duration = 55, Id = "dsds", InterestRate = 5 },
                    new Loan { Amount = 10, CreditRating = "A+", Duration = 45, Id = "dsdsaa", InterestRate = 51 },
                    new Loan { Amount = 10, CreditRating = "A+", Duration = 65, Id = "dsdsggaa", InterestRate = 15 },
                    new Loan { Amount = 10, CreditRating = "C", Duration = 65, Id = "dsdsggaa", InterestRate = 15 },
                    new Loan { Amount = 10, CreditRating = "C", Duration = 65, Id = "dsdsggaa", InterestRate = 15 },
                    new Loan { Amount = 10, CreditRating = "A", Duration = 65, Id = "dsdsggaa", InterestRate = 15 },
                    new Loan { Amount = 10, CreditRating = "D", Duration = 65, Id = "dsdsggaa", InterestRate = 15 },

                }
            });

            var issuerCfg = new LoanIssuerClientConfiguration
            {
                LoanIssuer = LoanIssuer.PaskoluKlubas,
                Login = login.Text,
                Password = password.Password
            };


            _loanChecker = PeriodicLoanCheckerBuilder
                .SetLoanIssuer(issuerCfg)
                .CheckEvery(TimeSpan.FromMinutes(1))
                .CallOnNewLoans(toastMessenger.ShowToastMessage)
                .Build();

            try
            {
                _loanChecker.Start();
            }
            catch (Exception ex)
            {

                throw;
            }


            // _loanChecker.Stop();




            //creds check
            //var isOk = await PKLoanIssuerClient
            //    .WithCredentials(login.Text, password.Password)
            //    .IsAbleToLoginAsync();

            //var monitor = LoanListMonitorFactory.Create(new[]
            //{
            //    new LoanIssuerClientConfiguration
            //    {
            //        LoanIssuer = LoanIssuer.PaskoluKlubas,
            //        Login = login.Text,
            //        Password = password.Password
            //    },
            //    new LoanIssuerClientConfiguration
            //    {
            //         LoanIssuer = LoanIssuer.Finbee,
            //         Login = "",
            //         Password = ""
            //    }
            //});

            //var results = await monitor.GetNewLoansAsync();
            //var results2 = await monitor.GetNewLoansAsync();


            //var i = 0;
            //        var isOk = await _paskoluKlubasLoanWatcher.IsAbleToLoginAsync(login.Text, password.Password);

            //        var loans = new List<Loan>();

            //        var baseAddress = new Uri("https://www.paskoluklubas.lt/");
            //        var cookieContainer = new CookieContainer();
            //        using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            //        using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            //        {
            //            //usually i make a standard request without authentication, eg: to the home page.
            //            //by doing this request you store some initial cookie values, that might be used in the subsequent login request and checked by the server
            //            var homePageResult = await client.GetAsync("/");
            //            homePageResult.EnsureSuccessStatusCode();

            //            var loginFormResult = await client.GetAsync("/prisijungti");

            //            var r1 = await loginFormResult.Content.ReadAsStreamAsync();

            //            var formDoc = new HtmlDocument();
            //            formDoc.Load(r1);

            //            var cookieConsent = formDoc.GetElementbyId("cookie_consent__token");
            //            var ccvalue = cookieConsent.GetAttributes("value").FirstOrDefault().Value;

            //            var consentContent = new FormUrlEncodedContent(new[]
            //            {
            //            //the name of the form values must be the name of <input /> tags of the login form, in this case the tag is <input type="text" name="username">
            //            new KeyValuePair<string, string>("cookie_consent[_token]", ccvalue)
            //            });

            //            var consentResult = await client.PostAsync("/cookie/consent", consentContent);
            //            consentResult.EnsureSuccessStatusCode();

            //            var t = formDoc.GetElementbyId("_csrf_token");
            //            var v = t.GetAttributes("value").FirstOrDefault().Value;

            //            var content = new FormUrlEncodedContent(new[]
            //            {
            //            //the name of the form values must be the name of <input /> tags of the login form, in this case the tag is <input type="text" name="username">
            //            //new KeyValuePair<string, string>("_username", "d.savickij@hotmail.com"),
            //            //new KeyValuePair<string, string>("_password", "Dell@4182200"),
            //            new KeyValuePair<string, string>("_username", login.Text),
            //            new KeyValuePair<string, string>("_password", password.Password),
            //            new KeyValuePair<string, string>("_csrf_token", v),
            //            new KeyValuePair<string, string>("step", "first"),
            //});
            //            var loginResult = await client.PostAsync("/prisijungti", content);
            //            loginResult.EnsureSuccessStatusCode();


            //            var c = cookieContainer.GetCookies(baseAddress);

            //            var r2 = await loginResult.Content.ReadAsStreamAsync();

            //            var doc1 = new HtmlDocument();
            //            doc1.Load(r2);
            //            var txt = doc1.Text;


            //            //make the subsequent web requests using the same HttpClient object

            //            //var result = await client.GetAsync("/user/account/statistics");


            //            var result = await client.GetAsync("/investor/investment/list/loan/requests");

            //            var r = await result.Content.ReadAsStreamAsync();

            //            var doc = new HtmlDocument();
            //            doc.Load(r);

            //            //var el = doc.GetElementbyId("primary_loans_for_investor_list");
            //            //var table = el.ChildNodes.FindFirst("table").ChildNodes.FindFirst("tbody").ChildNodes;

            //            //var rows = table.SelectNodes("/");



            //            var rows = doc.GetElementbyId("primary_loans_for_investor_list").SelectNodes("table[1]/tbody[1]/tr");

            //            foreach (var row in rows)
            //            {
            //                //var id = row.SelectNodes("td[1]/div[1]/a[1]/span").First().InnerText;
            //                //var rating = row.SelectNodes("td[2]/span").First().InnerText.Trim(new[] { ' ', '\n', '\r' });
            //                //var rate = double.Parse(row.SelectNodes("td[3]/div/div").First().InnerText.Trim(new[] { '%' }));

            //                //var duration = int.Parse(row.SelectNodes("td[4]/div").First().InnerText.Split(' ')[0]);
            //                //var amount = int.Parse(row.SelectNodes("td[5]/div").First().InnerText.Split('.')[0]);

            //                loans.Add(new Loan
            //                {
            //                    Id = row.SelectNodes("td[1]/div[1]/a[1]/span").First().InnerText,
            //                    CreditRating = row.SelectNodes("td[2]/span").First().InnerText.Trim(new[] { ' ', '\n', '\r' }),
            //                    InterestRate = double.Parse(row.SelectNodes("td[3]/div/div").First().InnerText.Trim(new[] { '%' })),
            //                    Duration = int.Parse(row.SelectNodes("td[4]/div").First().InnerText.Split(' ')[0]),
            //                    Amount = int.Parse(row.SelectNodes("td[5]/div").First().InnerText.Split('.')[0])
            //                });
            //            }

            //            //foreach (HtmlNode row in table.SelectNodes(@"\"))
            //            //{
            //            //    Console.WriteLine("Found: " + table.Id);
            //            //    foreach (HtmlNode ro in table.SelectNodes("tr"))
            //            //    {
            //            //        Console.WriteLine("row");
            //            //        foreach (HtmlNode cell in row.SelectNodes("th|td"))
            //            //        {
            //            //            Console.WriteLine("cell: " + cell.InnerText);
            //            //        }
            //            //    }
            //            //}

            //        }



            //var b = new ToastContentBuilder()
            //    .AddArgument("conversationId", 9813)
            //    .AddText("Some text")
            //    .AddButton(new ToastButton()
            //    .SetContent("Archive")
            //    .AddArgument("action", "archive")
            //    .SetBackgroundActivation())
            //    .AddAudio(new Uri("ms-appx:///Sound.mp3"));

            //b.Show();

            //var msg = GetToastMessageText(loans);

            //var toast = new ToastContentBuilder()
            //    .AddHeader("paskolu-klubas", "New loan requests in Paskolu Klubas", "new-loans-pk")
            //    .AddButton(new ToastButtonDismiss())
            //    .AddText(msg);
            ////.AddText("fdfdf")
            ////.AddText($"A    5", AdaptiveTextStyle.Body)
            ////.AddText($"A+   5", AdaptiveTextStyle.Body);






            //toast.Show();


        }

        private string GetToastMessageText(IEnumerable<Loan> loans)
        {
            var loanGroups = loans.GroupBy(x => x.CreditRating).OrderBy(x => x.Key);

            var sb = new StringBuilder();

            foreach (var loanGroup in loanGroups)
            {
                sb.AppendLine($"{loanGroup.Key,-20} | {loanGroup.Count()}");
            }

            return sb.ToString();
        }

        private void login_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (login.Text.Length != 0 && password.Password.Length != 0)
            {
                watcher_start.IsEnabled = true;
            }

        }

        private void password_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (login.Text.Length != 0 && password.Password.Length != 0)
            {
                watcher_start.IsEnabled = true;
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}
