using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using IRISChatServer.Configs;

namespace IRISChatServer.DataAccessLayer.Encryption
{
    public class AES256
    {
        #region "Extensions"
        public static string BytesToString(byte[] Input)
        {
            return Encoding.UTF8.GetString(Input);
        }
        public static byte[] StringToBytes(string Input)
        {
            return Encoding.UTF8.GetBytes(Input);
        }
        public static byte[] Encrypt(byte[] Input)
        {
            return Encrypt(Input, StringToBytes(DataAccessLayerConfig.AES_KEY), Convert.FromBase64String(DataAccessLayerConfig.AES_IV));
        }
        public static byte[] Decrypt(byte[] Input)
        {
            return Decrypt(Input, StringToBytes(DataAccessLayerConfig.AES_KEY), Convert.FromBase64String(DataAccessLayerConfig.AES_IV));
        }
        public static byte[] Encrypt(string Input)
        {
            return Encrypt(StringToBytes(Input), StringToBytes(DataAccessLayerConfig.AES_KEY), Convert.FromBase64String(DataAccessLayerConfig.AES_IV));
        }
        public static byte[] Decrypt(string Input)
        {
            return Decrypt(StringToBytes(Input), StringToBytes(DataAccessLayerConfig.AES_KEY), Convert.FromBase64String(DataAccessLayerConfig.AES_IV));
        }
        #endregion

        #region "Encryption / Decryption"
        public static byte[] Encrypt(byte[] Input, byte[] Key, byte[] IV)
        {
            byte[] encryptedData;
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(Input, 0, Input.Length);
                    }
                    encryptedData = msEncrypt.ToArray();
                }
            }
            return encryptedData;
        }
        public static byte[] Decrypt(byte[] Input, byte[] Key, byte[] IV)
        {
            byte[] decryptedData;
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream())
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Write))
                    {
                        csDecrypt.Write(Input, 0, Input.Length);
                    }
                    decryptedData = msDecrypt.ToArray();
                }
            }
            return decryptedData;
        }
        #endregion
    }
}