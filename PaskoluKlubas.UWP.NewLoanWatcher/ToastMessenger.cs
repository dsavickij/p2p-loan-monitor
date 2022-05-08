using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Toolkit.Uwp.Notifications;

namespace PaskoluKlubas.UWP.NewLoanWatcher
{
    public class ToastMessenger
    {
        public void ShowToastMessages(IDictionary<LoanIssuer, IEnumerable<Loan>> loans)
        {
            var b = new ToastContentBuilder()
                .AddArgument("conversationId", 9813)
                .AddText("Some text")
                .AddButton(new ToastButton()
                .SetContent("Archive")
                .AddArgument("action", "archive")
                .SetBackgroundActivation())
                .AddAudio(new Uri("ms-appx:///Sound.mp3"));

            b.Show();
        }
    }
}