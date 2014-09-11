///
/// <author>lufty.abdillah@gmail.com</author>
/// <summary>
/// Toyota .Net Development Kit
/// Copyright (c) Toyota Motor Manufacturing Indonesia, All Right Reserved.
/// </summary>
///
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Toyota.Common.Utilities;
using System.IO;

namespace Toyota.Common.Web.Platform
{
    public class AES256EncryptionAgent: IEncryptionAgent
    {        
        public AES256EncryptionAgent()
        {
            
        }


        public byte[] Encrypt(string text)
        {
            AesManaged provider = _CreateProvider();

            MemoryStream memstream = new MemoryStream();
            CryptoStream cryptstream = new CryptoStream(memstream, provider.CreateEncryptor(provider.Key, provider.IV), CryptoStreamMode.Write);
            StreamWriter writer = new StreamWriter(cryptstream);
            writer.Write(text);
            writer.Close();

            byte[] encrypted = memstream.ToArray();            
            memstream.Close();

            return encrypted;
        }
        public string Decrypt(byte[] cipher)
        {
            AesManaged provider = _CreateProvider();

            MemoryStream memstream = new MemoryStream(cipher);
            CryptoStream cryptstream = new CryptoStream(memstream, provider.CreateDecryptor(provider.Key, provider.IV), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(cryptstream);
            string encryptedText = reader.ReadToEnd();
            reader.Close();

            return encryptedText;
        }               

        private AesManaged _CreateProvider()
        {
            AesManaged provider = new AesManaged();
            byte[] saltBytes = Encoding.UTF8.GetBytes(GlobalConstants.Instance.SECURITY_SALT_SESSION_ID);
            provider.Key = saltBytes;
            provider.IV = saltBytes;

            return provider;
        }
    }
}
