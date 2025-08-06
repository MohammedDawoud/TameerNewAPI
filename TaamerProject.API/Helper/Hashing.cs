using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace TaamerProject.API.Helper
{
    public class Hashing
    {
        public string Hash_EncryptValue(string value)
        {
            string hash = "f0xle@rn";
            byte[] data = Encoding.UTF8.GetBytes(value);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDesc = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform cryptoTransform = tripDesc.CreateEncryptor();
                    byte[] result = cryptoTransform.TransformFinalBlock(data, 0, data.Length);
                    return Convert.ToBase64String(result, 0, result.Length);
                }
            }
        }
        public string Hash_DecryptValue(string value)
        {
            string hash = "f0xle@rn";
            byte[] data = Convert.FromBase64String(value); ;
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDesc = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform cryptoTransform = tripDesc.CreateDecryptor();
                    byte[] result = cryptoTransform.TransformFinalBlock(data, 0, data.Length);
                    return Encoding.UTF8.GetString(result);
                }
            }
        }
    }
}