namespace P2PLending.LoanMonitor.Core.Models
{
    public struct Loan
    {
        public string Id { get; set; }
        public string CreditRating { get; set; }
        public double InterestRate { get; set; }
        public int Duration { get; set; }
        public int Amount { get; set; }
    }
}