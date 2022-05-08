using System.Threading.Tasks;

public interface ILoanIssuerLoginCheck
{
    Task<bool> IsAbleToLoginAsync();
}
