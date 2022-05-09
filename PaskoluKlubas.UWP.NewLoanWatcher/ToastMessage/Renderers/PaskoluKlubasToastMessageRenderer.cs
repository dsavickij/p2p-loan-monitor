using System.Linq;
using System;
using Microsoft.Toolkit.Uwp.Notifications;
using System.Text;

namespace PaskoluKlubas.UWP.NewLoanWatcher
{
    public class PaskoluKlubasToastMessageRenderer : IToastMessageRenderer
    {
        public void ShowToastMessage(LoanListing issuerLoans)
        {
            var msg = GetMessage(issuerLoans);
            ShowToastMessage(msg);
        }

        private void ShowToastMessage(string msg)
        {
            var builder = new ToastContentBuilder()
                .AddHeader("loan-watcher-id", "Paskolu klubas issued new loans", string.Empty)
                .AddAppLogoOverride(new Uri("ms-appx:///Assets/paskolu_klubas_logo.png"))
                //.AddHeroImage(new Uri("ms-appx:///Assets/paskolu_klubas_logo.png"))
                //.AddInlineImage(new Uri("ms-appx:///Assets/paskolu_klubas_logo.png"))
                .AddArgument("conversationId", 9813)
                .AddText(msg)
                .AddButton(new ToastButtonDismiss())
                .AddButton(new ToastButton()
                .SetContent("View in browser")
                .AddArgument("action", "archive")
                .SetBackgroundActivation())
                .AddAudio(new Uri("ms-appx:///Sound.mp3"));

            builder.Show();
        }

        private string GetMessage(LoanListing issuerLoans)
        {
            var loanGroups = issuerLoans.Loans.GroupBy(x => x.CreditRating).OrderBy(x => x.Key);

            var sb = new StringBuilder();

            foreach (var loanGroup in loanGroups)
            {
                sb.AppendLine($"{loanGroup.Key,-20} | {loanGroup.Count()}");
            }

            return sb.ToString();
        }
    }
}