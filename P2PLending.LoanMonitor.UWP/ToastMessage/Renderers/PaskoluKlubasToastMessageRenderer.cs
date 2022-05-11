//using System;
//using Microsoft.Toolkit.Uwp.Notifications;

//namespace PaskoluKlubas.UWP.NewLoanWatcher
//{
//    public class PaskoluKlubasToastMessageRenderer : ToastMessageRenderer
//    {
//        protected override void ShowToastMessage(string msg)
//        {
//            var builder = new ToastContentBuilder()
//                .AddHeader("loan-watcher-id", string.Empty, string.Empty)      
//                .AddAppLogoOverride(new Uri("ms-appx:///Assets/paskolu_klubas_logo.png"), ToastGenericAppLogoCrop.Circle)
//                .AddArgument("conversationId", 9813)
//                .AddText("New loans in Paskolu Klubas")
//                .AddText(msg, AdaptiveTextStyle.Body)
//                .AddButton(new ToastButtonDismiss())
//                .AddButton(new ToastButton()
//                .SetContent("View in browser")
//                .AddArgument("action", "archive")
//                .SetBackgroundActivation())
//                .AddAudio(new Uri("ms-appx:///Sound.mp3"));

//            builder.Show();
//        }
//    }
//}