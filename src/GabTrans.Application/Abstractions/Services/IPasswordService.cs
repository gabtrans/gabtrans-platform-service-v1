

namespace GabTrans.Application.Abstractions.Services
{
    public interface IPasswordService
    {
        string CreateRandomKey();
        string Generate(int length);
        string Generate(int minLength, int maxLength);
        string GenerateRandomLetter(int length);
        string GenerateLetter(int minLength, int maxLength);
    }
}
