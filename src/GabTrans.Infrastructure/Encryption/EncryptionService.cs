using GabTrans.Application.Abstractions.Services;
using System.Security.Cryptography;
using System.Text;


namespace GabTrans.Infrastructure.Encryption
{
    public class EncryptionService : IEncryptionService
    {
        private static readonly string key = "5491a105-1abf-4f25-ae11-01e823bd129f";

        private const int urlIdEncodeSalt = 13411;
        private static readonly byte[] initVectorBytes = Encoding.ASCII.GetBytes("tu89geji340t89u2");

        // This constant is used to determine the keysize of the encryption algorithm.
        private const int keysize = 256;

        public string EncryptPassword(string text, int workFactor = 13)
        {
            return BCrypt.Net.BCrypt.HashPassword(text, workFactor);
        }

        public bool VerifyHash(string text, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(text, hash);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public string Encrypt(string plainText)
        {
            string passPhrase = "Boy_toy^419!@#";

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null))
            {
                byte[] keyBytes = password.GetBytes(keysize / 8);
                using var symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;
                using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                            cryptoStream.FlushFinalBlock();
                            byte[] cipherTextBytes = memoryStream.ToArray();
                            return Convert.ToBase64String(cipherTextBytes);
                        }
                    }
                }
            }
        }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public string Decrypt(string cipherText)
        {
            string passPhrase = "Boy_toy^419!@#";
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            using (PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null))
            {
                byte[] keyBytes = password.GetBytes(keysize / 8);
                using var symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;
                using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes))
                {
                    using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                        }
                    }
                }
            }
        }


        public string Decode(string text)
        {
            text = text.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(text);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(key, new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    text = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return text;
        }

        public string Encode(string text)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(text);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    text = Convert.ToBase64String(ms.ToArray());
                }
            }
            return text;
        }

        public string MaskPrefix(string value, int count = 6)
        {
            int length = Math.Min(value.Length, count);

            return new string('*', length) + string.Concat(value.Skip(length));
        }


        public string HMACSHA256(string data, string key)
        {
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.ASCII.GetBytes(key)))
            {
                byte[] hashedBytes = hmac.ComputeHash(Encoding.ASCII.GetBytes(data));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public string ToSha256(string input)
        {
            var enc = Encoding.UTF8;
            var hashBuilder = new StringBuilder();
            using var hash = SHA256.Create();
            byte[] result = hash.ComputeHash(enc.GetBytes(input));
            foreach (var b in result)
                hashBuilder.Append(b.ToString("x2"));
            return hashBuilder.ToString();
        }

        public string SHA512_ComputeHash(string text, string secretKey)
        {
            var hash = new StringBuilder();
            byte[] secretkeyBytes = Encoding.UTF8.GetBytes(secretKey);
            byte[] inputBytes = Encoding.UTF8.GetBytes(text);
            using (var hmac = new HMACSHA512(secretkeyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();
        }

        public string ToSha512(string token)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(token);
            using var hash = System.Security.Cryptography.SHA512.Create();
            var hashedInputBytes = hash.ComputeHash(bytes);

            var hashedInputStringBuilder = new System.Text.StringBuilder(128);
            foreach (var b in hashedInputBytes)
                hashedInputStringBuilder.Append(b.ToString("X2"));
            return hashedInputStringBuilder.ToString();
        }

        public string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }


        public String Encrypt(String word, String key, String iv)
        {
            byte[] result = null;
            // string word = "Joscool";
            byte[] wordBytes = Encoding.UTF8.GetBytes(word);
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    //string s = AES.BlockSize / (double)8;
                    AES.BlockSize = 128;
                    AES.Key = Encoding.UTF8.GetBytes(key);
                    AES.IV = Encoding.UTF8.GetBytes(iv);

                    AES.Mode = System.Security.Cryptography.CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(wordBytes, 0, wordBytes.Length);
                        cs.Close();
                    }
                    byte[] encryptedBytes = ms.ToArray();
                    result = encryptedBytes;
                    string result2 = Convert.ToBase64String(encryptedBytes);
                    return ByteArrayToString(encryptedBytes).ToUpper();
                }
            }
        }




        public string Decrypt(String word, String key, String iv)
        {
            //ErrorLogManager.LogError("AesEncryptManager", "", "", word + "|" + key + "|" + iv);
            // string word = "A7BA53AAA7D67CA8CC54913DA398E189";
            // byte[] wordBytes = cipher;//StringToByteArray(word);
            byte[] wordBytes = StringToByteArray(word);
            byte[] byteBuffer = new byte[wordBytes.Length - 1 + 1];
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = Encoding.UTF8.GetBytes(key);
                    AES.IV = Encoding.UTF8.GetBytes(iv);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(wordBytes, 0, wordBytes.Length);
                        cs.Close();
                    }
                    byte[] decryptedBytes = ms.ToArray();
                    return System.Text.Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }




        public string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }




        public byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
        }

        public string EncodeWithKey(string text)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(text);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    text = Convert.ToBase64String(ms.ToArray());
                }
            }
            return text;
        }
    }
}
