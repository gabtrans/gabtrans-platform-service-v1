

namespace GabTrans.Application.Abstractions.Services
{
    public interface ISequenceService
    {
        string GenerateRandomNumber();
        string GenerateRandomNumber(int length = 6);
        string Settlement(long transactionType);
        string GenerateAccountNumber();
        string GenerateRandomString();
    }
}
