

namespace GabTrans.Application.Abstractions.Services
{
    public interface IEncryptionService
    {
        string ToSha512(string token);
        string HMACSHA256(string data, string key);
        string SHA512_ComputeHash(string text, string secretKey);
        string Decode(string text);
        string Encode(string text);
        string MaskPrefix(string value, int count = 6);
        string ToSha256(string input);
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
        bool VerifyHash(string text, string hash);
        string EncryptPassword(string text, int workFactor = 13);
        string Base64Encode(string plainText);
        string EncodeWithKey(string text);
        string Base64Decode(string base64EncodedData);
        string Decrypt(String word, String key, String iv);
        String Encrypt(String word, String key, String iv);
    }
}
