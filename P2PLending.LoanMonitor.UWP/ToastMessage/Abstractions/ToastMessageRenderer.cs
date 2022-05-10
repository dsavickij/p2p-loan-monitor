using System.Linq;
using System.Text;

namespace PaskoluKlubas.UWP.NewLoanWatcher
{
    public abstract class ToastMessageRenderer
    {
        public virtual void ShowToastMessage(LoanListing issuerLoans)
        {
            var msg = GetMessage(issuerLoans);
            ShowToastMessage(msg);
        }

        protected abstract void ShowToastMessage(string msg);

        protected string GetMessage(LoanListing issuerLoans)
        {
            var loanGroups = issuerLoans.Loans.GroupBy(x => x.CreditRating).OrderBy(x => x.Key);

            var sb = new StringBuilder();

            foreach (var loanGroup in loanGroups)
            {
                sb.AppendLine($"+{loanGroup.Count()} with rating {loanGroup.Key}");
            }

            return sb.ToString();
        }
    }
}