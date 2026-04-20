using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto;
using System.Text;
using Org.BouncyCastle.OpenSsl;
using System.Security.Cryptography;
using GabTrans.Domain.Models;
using GabTrans.Application.Abstractions.Services;

namespace GabTrans.Application.Services
{
    public class RsaEncryptionService: IRsaEncryptionService
    {
        //public string RsaDecryptWithPublic(string base64Input, string publicKey)
        //{
        //    var bytesToDecrypt = Convert.FromBase64String(base64Input);
        //    var decryptEngine = new Pkcs1Encoding(new RsaEngine());
        //    using (var txtreader = new System.IO.StringReader(publicKey))
        //    {
        //        var keyParameter = (AsymmetricKeyParameter)new PemReader(txtreader).ReadObject();
        //        decryptEngine.Init(false, keyParameter);
        //    }
        //    var decrypted = Encoding.UTF8.GetString(decryptEngine.ProcessBlock(bytesToDecrypt, 0, bytesToDecrypt.Length));
        //    return decrypted;
        //}
        //public string RsaEncryptWithPublic(string clearText, string publicKey)
        //{
        //    var bytesToEncrypt = Encoding.UTF8.GetBytes(clearText);
        //    var encryptEngine = new Pkcs1Encoding(new RsaEngine());
        //    using (var txtreader = new System.IO.StringReader(publicKey))
        //    {
        //        var keyParameter = (AsymmetricKeyParameter)new PemReader(txtreader).ReadObject();
        //        encryptEngine.Init(true, keyParameter);
        //    }
        //    var encrypted = Convert.ToBase64String(encryptEngine.ProcessBlock(bytesToEncrypt, 0, bytesToEncrypt.Length));
        //    return encrypted;
        //}
        public string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }


        //public SmileSignedObject SignRequest(string apiKey, string strPartnerID)
        //{
        //    var smileObject = new SmileSignedObject();

        //    // But for the sec_key calculation we need an int. 
        //    int partnerID = Int32.Parse(strPartnerID);

        //    // Get a Uxix timestamp
        //    Int32 timestamp = (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

        //    string plaintext = partnerID.ToString() + ":" + timestamp.ToString();
        //    Console.WriteLine("plaintext: " + plaintext);

        //    // Hash the plaintext
        //    SHA256 mySHA256 = SHA256.Create();
        //    var bytesToEncrypt = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(plaintext));

        //    var encrypted = RsaEncryptWithPublic(ByteArrayToHexString(bytesToEncrypt), apiKey);

        //    string sec_key = encrypted + "|" + ByteArrayToHexString(bytesToEncrypt);

        //    smileObject.Signature=sec_key;

        //    // Use sec_key and timestamp in sending messages to the server.

        //    return smileObject;
        //}
    }
}
