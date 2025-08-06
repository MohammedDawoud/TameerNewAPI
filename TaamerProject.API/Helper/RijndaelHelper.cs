using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Drawing;

namespace TaamerProject.API.Helper
{
    public class RijndaelHelper
    {
        public static bool EncryptFile(string inputFile, string outputFile)
        {
            try
            {
                if (System.IO.File.Exists(outputFile))
                {
                    System.IO.File.Delete(outputFile);
                }
                // string password = @"myKey123"; // Your Key Here 8 letters
                string password = @"myKe1347"; // Your Key Here
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                string cryptFile = outputFile;
                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, key),
                    CryptoStreamMode.Write);

                FileStream fsIn = new FileStream(inputFile, FileMode.Open);

                int data;
                while ((data = fsIn.ReadByte()) != -1)
                    cs.WriteByte((byte)data);


                fsIn.Close();
                cs.Close();
                fsCrypt.Close();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public static bool DecryptFile(string inputFile, string outputFile)
        {
            try
            {
                //string password = @"myKey123"; // Your Key Here
                string password = @"myKe1347"; // Your Key Here
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);
               
                GC.Collect();
                GC.WaitForPendingFinalizers();
                
                FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateDecryptor(key, key),
                    CryptoStreamMode.Read);

                FileStream fsOut = new FileStream(outputFile, FileMode.Create);

                int data;
                while ((data = cs.ReadByte()) != -1)
                    fsOut.WriteByte((byte)data);

                fsOut.Close();
                cs.Close();
                fsCrypt.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}


